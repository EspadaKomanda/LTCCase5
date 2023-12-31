using AuthDbService.Database;
using AuthDbService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
//Console.WriteLine("AASAS");
using (ApplicationContext ctx = new ApplicationContext())
{
    ctx.users.FirstOrDefault();
}
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<AuthDbService.Services.AuthDbService>();
// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();