using Grpc.Core;
using PersonCRUD.Domain.Contract;
using PersonCRUD.Domain.Models;
using PersonCRUD.Infrastructure.Data;
using PersonCRUD.Protos;

namespace PersonCRUD.Services
{
    public class PersonService : Person.PersonBase
    {
        private readonly ILogger<PersonService> _logger;
        private readonly PersonDbContext _context;

        private readonly IRepository _repository;

        public PersonService(ILogger<PersonService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async override Task<PersonReply> GetPerson(PersonRequest request, ServerCallContext context)
        {
            // 1. Retrieve the person from your data store based on the ID
            var person = await _repository.GetByIDAsync(request.ID);

            // 2. Check if the person exists
            if (person == null)
            {
                return new PersonReply { Message = "Person not found.", StatusID = 0 };
            }

            // 3. Create the PersonReply object
            var reply = new PersonReply
            {
                ID = (int)person.ID,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                BirthDate = person.BirthDate.ToString(),
                Message = "Person retrieved successfully.",
                StatusID = 1
            };

            return reply;
        }
        public async override Task<PersonListReply> GetPersonList(Empty empty, ServerCallContext context)
        {
            var personList = await _repository.GetAllAsync();

            PersonListReply replyList = new();

            foreach (var person in personList)
            {
                replyList.People.Add(new PersonReply
                {
                    ID = (int)person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    NationalNo = person.NationalNo,
                    BirthDate = person.BirthDate.ToString(),
                    Message = "Person retrieved successfully.",
                    StatusID = 1
                });
            }
            return replyList;
        }

        public async override Task<PersonReply> CreatePerson(CreatePersonRequest request, ServerCallContext context)
        {
            try
            {
                var person = new Individual
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    NationalNo = request.NationalNo,
                    BirthDate = DateTime.Parse(request.BirthDate)
                };

                await _repository.AddAsync(person);

                return new PersonReply
                {
                    ID = (int)person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    NationalNo = person.NationalNo,
                    BirthDate = person.BirthDate.ToString(),
                    Message = "Person created successfully.",
                    StatusID = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person.");
                var res = new PersonReply
                {
                    Message = $"Error creating person: {ex.Message}",
                    StatusID = 500
                };
                return res;
            }
        }

        public async override Task<PersonReply> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
        {
            try
            {
                var person = await _repository.GetByIDAsync(request.ID);
                if (person == null)
                {
                    return new PersonReply
                    {
                        Message = "Person not found.",
                        StatusID = 404
                    };
                }

                person.FirstName = request.FirstName ?? person.FirstName;
                person.LastName = request.LastName ?? person.LastName;
                person.NationalNo = request.NationalNo ?? person.NationalNo;
                person.BirthDate = DateTime.TryParse(request.BirthDate, out var birthDate) ? birthDate : person.BirthDate;

                await _repository.UpdateAsync(person);

                return new PersonReply
                {
                    ID = (int)person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    NationalNo = person.NationalNo,
                    BirthDate = person.BirthDate.ToString(),
                    Message = "Person updated successfully.",
                    StatusID = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating person.");
                return new PersonReply
                {
                    Message = $"Error updating person: {ex.Message}",
                    StatusID = 500
                };
            }
        }

        public async override Task<PersonReply> DeletePerson(PersonRequest request, ServerCallContext context)
        {
            try
            {
                var person = await _repository.GetByIDAsync(request.ID);
                if (person == null)
                {
                    return new PersonReply
                    {
                        Message = "Person not found.",
                        StatusID = 404
                    };
                }

                await _repository.DeleteAsync(person.ID);

                return new PersonReply
                {
                    Message = "Person deleted successfully.",
                    StatusID = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting person.");
                return new PersonReply
                {
                    Message = $"Error deleting person: {ex.Message}",
                    StatusID = 500
                };
            }
        }
    }
}
