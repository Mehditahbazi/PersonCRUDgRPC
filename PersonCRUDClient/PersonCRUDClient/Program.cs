using Grpc.Net.Client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7042");
//var client = new PersonCRUDClient.CreatePersonRequest(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });
//Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();