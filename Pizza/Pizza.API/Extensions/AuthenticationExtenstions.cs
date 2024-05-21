using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Pizza.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationExtension(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5144/", // Specify the valid issuer URL
                    ValidAudience = "http://localhost:5144/", // Specify the valid audience URL
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretKey123456789101112131415!#"))
                };
            });

            return services; // Return the updated IServiceCollection
        }
    }
}
