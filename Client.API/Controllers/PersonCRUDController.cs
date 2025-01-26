using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using PersonCRUD.Protos;


namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonCRUDController : ControllerBase

    {
        private readonly GrpcChannel _channel;
        private readonly Person.PersonClient _client;
        private readonly IConfiguration _configuration;

        public PersonCRUDController(IConfiguration configuration)
        {
            _configuration = configuration;
            _channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcSettings:PersonServiceUrl"));
            _client = new Person.PersonClient(_channel);
        }

        [HttpGet("getPersonlist")]
        public async Task<PersonListReply> GetPersonListAsync()
        {
            Empty empty = new Empty();
            try
            {
                var response = await _client.GetPersonListAsync(empty);

                return response;
            }
            catch
            {

            }
            return null;
        }

        [HttpGet("getPersonbyid")]
        public async Task<PersonReply> GetPersonByIdAsync(int ID)
        {
            try
            {
                var request = new PersonRequest
                {
                    ID = ID
                };

                var response = await _client.GetPersonAsync(request);

                return response;
            }
            catch
            {

            }
            return null;
        }

        [HttpPost("addPerson")]
        public async Task<PersonReply> AddPersonAsync(CreatePersonRequest Person)
        {
            try
            {
                var response = await _client.CreatePersonAsync(new CreatePersonRequest
                {
                    FirstName = Person.FirstName,
                    LastName = Person.LastName,
                    BirthDate = Person.BirthDate,
                    NationalNo = Person.NationalNo,
                });

                return response;
            }
            catch
            {

            }
            return null;
        }

        [HttpPut("updatePerson")]
        public async Task<PersonReply> UpdatePersonAsync(UpdatePersonRequest Person)
        {
            try
            {
                var response = await _client.UpdatePersonAsync(new UpdatePersonRequest
                {
                    ID = Person.ID,
                    FirstName = Person.FirstName,
                    LastName = Person.LastName,
                    BirthDate = Person.BirthDate,
                    NationalNo = Person.NationalNo,
                });

                return response;
            }
            catch
            {

            }
            return null;
        }

        [HttpDelete("deletePerson")]
        public async Task<PersonReply> DeletePersonAsync(int Id)
        {
            try
            {
                var response = await _client.DeletePersonAsync(new PersonRequest { ID = Id });

                return response;
            }
            catch
            {

            }
            return null;
        }
    }
}