
using System.Text;
using MainService.Communicators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigCommunicator communicator = new ConfigCommunicator();
var Auth = communicator.GetAuth().Result;
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddSingleton<ConfigCommunicator>();
builder.Services.AddSingleton<AuthDbCommunicator>();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = Auth.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = Auth.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Auth.KEY)),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddRazorPages(options => options.RootDirectory = "/Pages/razorPages");
var app = builder.Build();
app.UseSession();

//add token to request header.


app.UseHttpsRedirection();

//add token to request header.
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    Console.WriteLine(token);
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
// Configure the HTTP request pipeline.

app.Run();
