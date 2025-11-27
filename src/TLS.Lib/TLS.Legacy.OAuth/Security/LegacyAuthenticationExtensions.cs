using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core;
using TLS.Legacy.OAuth.Security.Handlers;
using TLS.Legacy.OAuth.Security.Token;

namespace TLS.Legacy.OAuth.Security
{
    /// <summary>
    /// Simple .NET Core library to reading OWIN based OAuth tokens. 
    /// Just implemented the code that deserialize OWIN based token to ticket. 
    /// So, you can Authenticate your API user by old tokens on your ASPNET Core application. 
    /// Use the current OAuth mechanism of ASPNET Core for the new token generations.
    /// https://github.com/turgayozgur/Owin.Token.AspNetCore
    /// </summary>
    public static class LegacyAuthenticationExtensions
    {
        public static void AddLegacyAuthentication(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            //var decryptionKey = configuration.GetValue<string>("LegacyTokenAuthentication:DecryptionKey");
            //var validationKey = configuration.GetValue<string>("LegacyTokenAuthentication:ValidationKey");

            var options = new JwtBearerOptions
            {
                // other properties here
            };
            builder.AddJwtBearer(options =>
            {
                // Get configuration
                var configuration = services.GetConfiguration();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // You can change the parameters depends on your implementation.
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    //IssuerSigningKey =
                    //    new SymmetricSecurityKey(
                    //        Encoding.UTF8.GetBytes("The key(maybe guid) you specified when generating JwtBearer tokens"))
                };

                // Here is the important point! Add our fallback to SecurityTokenValidators list to validate OWIN tokens.
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new LegacyOAuthSecurityTokenHandler(new LegacyTokenAuthenticationOptions
                {
                    DecryptionKey = configuration.GetValue<string>("LegacyTokenAuthentication:DecryptionKey"),
                    ValidationKey = configuration.GetValue<string>("LegacyTokenAuthentication:ValidationKey")
                }));
            });
        }
    }
}
