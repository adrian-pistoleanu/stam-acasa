using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Quickstart
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the client is configured to use PKCE.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="client_id">The client identifier.</param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string client_id)
        {
            if (!string.IsNullOrWhiteSpace(client_id))
            {
                var client = await store.FindEnabledClientByIdAsync(client_id);
                return client?.RequirePkce == true;
            }

            return false;
        }

        /// <summary>
        /// Add a SSL certificate in production
        /// </summary>
        /// <param name="path">Calea stocata in appsettrings.json</param>
        /// <returns></returns>        
        public static IIdentityServerBuilder LoadSigningCredentialFrom(this IIdentityServerBuilder builder, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsProduction())
            {
                string sslCertPath = configuration["Ssl:Path"];
                string sslCertKey = configuration["Ssl:Key"];
                builder.AddSigningCredential(new X509Certificate2(sslCertPath, sslCertKey));
            }
            else
            {
                builder.AddDeveloperSigningCredential();
            }
            return builder;
        }
    }
}
