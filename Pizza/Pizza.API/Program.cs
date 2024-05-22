using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Pizza.API.Extensions;
using Pizza.Application.Core.Interfaces;
using Pizza.Application.Core.Services;
using Pizza.Data;
using Pizza.Infrastructure.Automapper;
using Pizza.Infrastructure.Data;
using Pizza.Infrastructure.Repository.Interfaces;
using Pizza.Infrastructure.Repository.Repos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
//Hämta värden från appsettings.json 
var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");

builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(),
                                        keyVaultClientId.Value!.ToString(),
                                        keyVaultClientSecret.Value!.ToString(),
                                        new DefaultKeyVaultSecretManager());

var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());

var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);

var connString = client.GetSecret("Myconnection").Value.Value.ToString();
builder.Services.AddDbContext<PizzaContext>(
    options => options.UseSqlServer(connString));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<PizzaContext>()
       .AddDefaultTokenProviders();

builder.Services.AddSwaggerExtension();
builder.Services.AddAuthenticationExtension(configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IDishRepo, DishRepo>();
builder.Services.AddScoped<IDishService, DishService>();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.AddSwaggerExtended();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
