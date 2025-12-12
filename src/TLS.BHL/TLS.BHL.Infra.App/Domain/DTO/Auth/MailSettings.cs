using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.DTO.Auth
{
    public class MailSettings
    {
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
    }
}
