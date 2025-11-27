using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Service;

namespace TLS.Legacy.OAuth.Connector
{
    public interface IOAuthConnectorService : IService
    {
        OAuthConnectorSettings Settings { get; }
        Task<OAuthCheckUserOutput> CheckUser(OAuthCheckUserInput input);
        Task<OAuthCreateUserOutput> CreateUser(OAuthCreateUserInput input);
        Task<OAuthUpdateUserOutput> UpdateUser(OAuthUpdateUserInput input);
    }
}
