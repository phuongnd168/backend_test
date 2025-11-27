using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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
    public static class LegacySecurityExtensions
    {
        public static int GetIdentityUserId(this IPrincipal principal)
        {
            int outVal = 0;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    int tmpVal;
                    if (int.TryParse(claim.Value, out tmpVal))
                    {
                        outVal = tmpVal;
                    }
                }
            }
            return outVal;
        }

        public static string GetIdentityUserName(this IPrincipal principal)
        {
            string outVal = null;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(ClaimTypes.Name);
                if (claim != null)
                {
                    outVal = claim.Value;
                }
            }
            return outVal;
        }

        public static string GetIdentityUserEmail(this IPrincipal principal)
        {
            string outVal = null;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(LegacyClaimTypes.UserEmail);
                if (claim != null)
                {
                    outVal = claim.Value;
                }
            }
            return outVal;
        }

        public static bool GetIdentityUserEmailConfirmed(this IPrincipal principal)
        {
            bool outVal = false;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(LegacyClaimTypes.UserEmailConfirmed);
                if (claim != null)
                {
                    bool tmpVal;
                    if (bool.TryParse(claim.Value, out tmpVal))
                    {
                        outVal = tmpVal;
                    }
                }
            }
            return outVal;
        }

        public static string GetIdentityUserPhoneNumber(this IPrincipal principal)
        {
            string outVal = null;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(LegacyClaimTypes.UserPhoneNumber);
                if (claim != null)
                {
                    outVal = claim.Value;
                }
            }
            return outVal;
        }

        public static bool GetIdentityUserPhoneNumberConfirmed(this IPrincipal principal)
        {
            bool outVal = false;
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst(LegacyClaimTypes.UserPhoneNumberConfirmed);
                if (claim != null)
                {
                    bool tmpVal;
                    if (bool.TryParse(claim.Value, out tmpVal))
                    {
                        outVal = tmpVal;
                    }
                }
            }
            return outVal;
        }

        public static string GetIdentity(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimDeviceKey = identity.FindFirst("as:client_id");
                if (claimDeviceKey != null)
                {
                    return claimDeviceKey.Value;
                }
            }
            return null;
        }

        public static string GetConnectAppId(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimDeviceKey = identity.FindFirst("as:client_id");
                if (claimDeviceKey != null)
                {
                    return claimDeviceKey.Value;
                }
            }
            return null;
        }

        public static string GetConnectDeviceKey(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimDeviceKey = identity.FindFirst(LegacyClaimTypes.ClaimTypeDeviceKey);
                if (claimDeviceKey != null)
                {
                    return claimDeviceKey.Value;
                }
            }
            return null;
        }

        public static string GetConnectDeviceName(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimDeviceKey = identity.FindFirst(LegacyClaimTypes.ClaimTypeDeviceName);
                if (claimDeviceKey != null)
                {
                    return claimDeviceKey.Value;
                }
            }
            return null;
        }

        public static string[] GetConnectDeviceGroups(this IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claimDeviceGroups = identity.FindFirst(LegacyClaimTypes.ClaimTypeDeviceGroups);
                if (claimDeviceGroups != null && !string.IsNullOrEmpty(claimDeviceGroups.Value))
                {
                    return claimDeviceGroups.Value.Split(',');
                }
            }
            return new string[] { };
        }
    }
}
