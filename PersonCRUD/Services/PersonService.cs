using Grpc.Core;
using PersonCRUD.Domain.Contract;
using PersonCRUD.Domain.Models;
using PersonCRUD.Infrastructure.Data;

namespace PersonCRUD.Services
{
    public class PersonService : Person.PersonBase
    {
        private readonly ILogger<PersonService> _logger;
        private readonly PersonDbContext _context;
        private readonly List<PersonRequest> _persons = [];
        private readonly IRepository _repository;
        private int _nextId = 1;
        public PersonService(ILogger<PersonService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public override async Task<PersonReply> GetPerson(PersonRequest request, ServerCallContext context)
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
        public async Task<List<PersonReply>> GetPersonList(Empty empty, ServerCallContext context)
        {
            var personList = await _repository.GetAllAsync();

            List<PersonReply> replyList = [];

            foreach (var person in personList)
            {
                replyList.Add(new PersonReply
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

        public Task<PersonReply> CreatePerson(CreatePersonRequest request, ServerCallContext context)
        {
            try
            {
                var person = new Individual // Your EF Core model
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    NationalNo = request.NationalNo,
                    BirthDate = DateTime.Parse(request.BirthDate)
                };

                _repository.AddAsync(person);

                return Task.FromResult(new PersonReply
                {
                    ID = (int)person.ID,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    NationalNo = person.NationalNo,
                    BirthDate = person.BirthDate.ToString(),
                    Message = "Person created successfully.",
                    StatusID = 200 // OK
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person.");
                var res = new PersonReply
                {
                    Message = $"Error creating person: {ex.Message}",
                    StatusID = 500 // Internal Server Error
                };
                return Task.FromResult(res);
            }
        }

        public async Task<PersonReply> UpdateItem(UpdatePersonRequest request, ServerCallContext context)
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

                person.FirstName = request.FirstName ?? person.FirstName; // Update only if a new value is provided.
                person.LastName = request.LastName ?? person.LastName;
                person.NationalNo = request.NationalNo ?? person.NationalNo;
                person.BirthDate = DateTime.TryParse(request.BirthDate, out var birthDate) ? birthDate : person.BirthDate;

                await _repository.UpdateAsync(person);
                //await _context.SaveChangesAsync();

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

        public async Task<PersonReply> DeleteItem(PersonRequest request, ServerCallContext context)
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

                _repository.DeleteAsync(person.ID);

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
