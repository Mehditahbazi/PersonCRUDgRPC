using Grpc.Net.Client;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PersonCRUD.Domain.Contract;
using PersonCRUD.Infrastructure.Data;
using PersonCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local")));
var connectionString = builder.Configuration.GetConnectionString("Local");
Console.WriteLine($"Connection String: {connectionString}");


// If using Kestrel, configure HTTP/2 for gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5001, o => o.Protocols = HttpProtocols.Http2);
    // Setup a HTTP/1.1 endpoint for other requests.
    options.ListenLocalhost(5000);
});

builder.Services.AddScoped<IRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

var app = builder.Build();
app.MapGrpcService<PersonService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


using var channel = GrpcChannel.ForAddress("https://localhost:7280");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

app.Run();
