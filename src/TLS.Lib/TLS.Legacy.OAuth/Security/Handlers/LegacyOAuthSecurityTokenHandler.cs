using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using TLS.Legacy.OAuth.Security.Token;

namespace TLS.Legacy.OAuth.Security.Handlers
{
    public class LegacyOAuthSecurityTokenHandler : SecurityTokenHandler
    {
        private readonly LegacyTokenAuthenticationOptions _options;

        public LegacyOAuthSecurityTokenHandler(LegacyTokenAuthenticationOptions options)
        {
            _options = options;
        }

        public override bool CanValidateToken => true;

        public override bool CanReadToken(string tokenString) => true;

        /// <summary>
        /// ValidateToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="validationParameters"></param>
        /// <param name="validatedToken"></param>
        /// <returns></returns>
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            // Get ticket
            var ticket = LegacyOAuthSecurityTokenHelper.GetTicket(token, _options);

            // Validate lifetime
            if (validationParameters.ValidateLifetime)
            {
                var expired = ticket.Identity.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Expired);
                if (expired != null && !string.IsNullOrEmpty(expired.Value))
                {
                    DateTime expiredDate;
                    if (DateTime.TryParse(expired.Value, out expiredDate))
                    {
                        if (DateTime.Now > expiredDate)
                        {
                            throw new SecurityTokenExpiredException(string.Format("Token expired at {0}", expiredDate.ToString("yyyy-MM-dd HH:mm:ss")));
                        }    
                    }    
                    else
                    {
                        throw new SecurityTokenInvalidLifetimeException(string.Format("Invalid expired claim {0}", expired.Value));
                    }    
                }
                else
                {
                    throw new SecurityTokenInvalidLifetimeException("Expired claim can not empty");
                }    
            }

            // Claim principal
            var claimsIdentity = new ClaimsIdentity(ClaimTypes.Email);//.AddClaims(ticket.Identity.Claims);
            claimsIdentity.AddClaims(ticket.Identity.Claims);

            validatedToken = default(SecurityToken);

            return new ClaimsPrincipal(claimsIdentity);
        }

        public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
        {
            throw new NotImplementedException();
        }

        public override void WriteToken(XmlWriter writer, SecurityToken token)
        {
            throw new NotImplementedException();
        }

        public override Type TokenType => typeof(SecurityToken);
    }
}
