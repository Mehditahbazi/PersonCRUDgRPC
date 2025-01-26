using PersonCRUD.Domain.Models;

namespace PersonCRUD.Domain.Contract;

public interface IRepository
{
    public Task<List<Individual>> GetAllAsync();
    public Task<Individual> GetByIDAsync(int id);
    public Task<Individual> AddAsync(Individual person);
    public Task<Individual> UpdateAsync(Individual person);
    public Task<bool> DeleteAsync(decimal id);
}
