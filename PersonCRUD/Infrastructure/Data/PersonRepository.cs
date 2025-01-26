using Microsoft.EntityFrameworkCore;
using PersonCRUD.Domain.Contract;
using PersonCRUD.Domain.Models;

namespace PersonCRUD.Infrastructure.Data;

public class PersonRepository : IRepository
{

    private readonly PersonDbContext _context;

    public PersonRepository(PersonDbContext context)
    {
        _context = context;
    }

    public async Task<List<Individual>> GetAllAsync()
    {
        return await _context.Individuals
            .Select(p => new Individual // Project to DTO
            {
                ID = p.ID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                NationalNo = p.NationalNo,
                BirthDate = p.BirthDate
            })
            .ToListAsync();
    }

    public async Task<Individual> GetByIDAsync(int id)
    {
        var person = await _context.Individuals.FindAsync(id);
        if (person == null) return null;
        return new Individual
        {
            ID = person.ID,
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalNo = person.NationalNo,
            BirthDate = person.BirthDate
        };
    }

    public async Task<Individual> AddAsync(Individual personDto)
    {
        var person = new Individual // Map DTO to Entity
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            NationalNo = personDto.NationalNo,
            BirthDate = personDto.BirthDate
        };

        _context.Individuals.Add(person);
        await _context.SaveChangesAsync();

        personDto.ID = person.ID; // set generated id to DTO
        return personDto; // Return the DTO with the generated ID
    }

    public async Task<Individual> UpdateAsync(Individual personDto)
    {
        var person = await _context.Individuals.FindAsync(personDto.ID);
        if (person == null) return null;

        person.FirstName = personDto.FirstName;
        person.LastName = personDto.LastName;
        person.NationalNo = personDto.NationalNo;
        person.BirthDate = personDto.BirthDate;

        await _context.SaveChangesAsync();
        return personDto;
    }

    public async Task<bool> DeleteAsync(decimal id)
    {
        var person = await _context.Individuals.FindAsync(id);
        if (person == null) return false;

        _context.Individuals.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }
}
