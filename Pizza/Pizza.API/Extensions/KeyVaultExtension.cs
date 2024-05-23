using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Pizza.Infrastructure.Data;

namespace Pizza.API.Extensions
{
    public static class KeyVaultExtension
    {
        public static IConfigurationBuilder ConfigureAzureKeyVault(this IConfigurationBuilder configurationBuilder, IConfiguration configuration)
        {
            var keyVaultURL = configuration.GetSection("KeyVault:KeyVaultURL");
            var keyVaultClientId = configuration.GetSection("KeyVault:ClientId");
            var keyVaultClientSecret = configuration.GetSection("KeyVault:ClientSecret");
            var keyVaultDirectoryID = configuration.GetSection("KeyVault:DirectoryID");

            configurationBuilder.AddAzureKeyVault(
                keyVaultURL.Value!.ToString(),
                keyVaultClientId.Value!.ToString(),
                keyVaultClientSecret.Value!.ToString(),
                new DefaultKeyVaultSecretManager()
            );

            return configurationBuilder;
        }

        public static IServiceCollection AddDbContextWithKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            var keyVaultURL = configuration.GetSection("KeyVault:KeyVaultURL");
            var keyVaultClientId = configuration.GetSection("KeyVault:ClientId");
            var keyVaultClientSecret = configuration.GetSection("KeyVault:ClientSecret");
            var keyVaultDirectoryID = configuration.GetSection("KeyVault:DirectoryID");

            var credential = new ClientSecretCredential(
                keyVaultDirectoryID.Value!.ToString(),
                keyVaultClientId.Value!.ToString(),
                keyVaultClientSecret.Value!.ToString()
            );
            var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);
            var connString = client.GetSecret("String").Value.Value.ToString();

            services.AddDbContext<PizzaContext>(options => options.UseSqlServer(connString));

            return services;
        }
    }
}



