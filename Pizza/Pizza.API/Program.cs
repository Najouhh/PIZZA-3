using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<PizzaContext>(
    options => options.UseSqlServer(@"Data Source=NAJAH\MSSQLSERVER01;Initial Catalog=Pizza3;Integrated Security=SSPI;TrustServerCertificate=True;")
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<PizzaContext>()
       .AddDefaultTokenProviders();

builder.Services.AddSwaggerExtension();
builder.Services.AddAuthenticationExtension();
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
