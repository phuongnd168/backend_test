using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Legacy.OAuth.Security
{
    public class LegacyClaimTypes
    {
        public const string UserEmail = "user:email";
        public const string UserEmailConfirmed = "user:email_confirmed";
        public const string UserPhoneNumber = "user:phone_number";
        public const string UserPhoneNumberConfirmed = "user:phone_number_confirmed";

        public const string ClaimTypeDeviceKey = "Dsvn:DeviceKey";
        public const string ClaimTypeDeviceName = "Dsvn:DeviceName";
        public const string ClaimTypeDeviceGroups = "Dsvn:DeviceGroups";

        public const string ClaimTypeLoginApp = "Dsvn:LoginApp";
        public const string ClaimTypeTwoFactor = "Dsvn:TwoFactorValue";

        public const string ClaimTypeClientId = "Dsvn:ClientId";
        public const string ClaimTypeClientCode = "Dsvn:ClientCode";
    }
}
