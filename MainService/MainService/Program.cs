
using MainService.Communicators;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddSingleton<ConfigCommunicator>();
builder.Services.AddSingleton<AuthDbCommunicator>();
builder.Services.AddRazorPages();

builder.Services.AddRazorPages(options => options.RootDirectory = "/Pages/razorPages");
var app = builder.Build();
app.MapRazorPages();
app.MapControllers();
// Configure the HTTP request pipeline.

app.Run();
