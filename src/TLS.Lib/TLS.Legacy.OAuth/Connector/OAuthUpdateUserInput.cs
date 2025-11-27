using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Legacy.OAuth.Connector
{
    public class OAuthUpdateUserInput : OAuthCreateUserInput
    {
        public int Id { get; set; }
    }
}
