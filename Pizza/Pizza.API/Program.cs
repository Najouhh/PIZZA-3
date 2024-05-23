using Microsoft.AspNetCore.Identity;
using Pizza.API.Extensions;
using Pizza.Data;
using Pizza.Infrastructure.Automapper;
using Pizza.Infrastructure.Data;
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

//keyvault config
builder.Configuration.ConfigureAzureKeyVault(builder.Configuration);
builder.Services.AddDbContextWithKeyVault(builder.Configuration);


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<PizzaContext>()
       .AddDefaultTokenProviders();

builder.Services.AddApplicationServices();
builder.Services.AddSwaggerExtension();
builder.Services.AddAuthenticationExtension(configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

app.AddSwaggerExtended();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
