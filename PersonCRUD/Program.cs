using Microsoft.EntityFrameworkCore;
using PersonCRUD.Domain.Contract;
using PersonCRUD.Infrastructure.Data;
using PersonCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local")));

builder.Services.AddScoped<IRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

var app = builder.Build();
app.MapGrpcService<PersonService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
