using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using TLS.Legacy.OAuth.Security.Handlers;
using TLS.Legacy.OAuth.Security.Token;

namespace TLS.Legacy.OAuth.Security.OAuth
{
    public static class OAuthJwtBearerExtensions
    {
        public static AuthenticationBuilder AddAuthenticationOwin(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            var decryptionKey = configuration.GetValue<string>("LegacyTokenAuthentication:DecryptionKey");
            var validationKey = configuration.GetValue<string>("LegacyTokenAuthentication:ValidationKey");

            var options = new JwtBearerOptions
            {
                // other properties here
            };
            return builder.AddJwtBearer(options =>
            {
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
