using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;

namespace TLS.BHL.Infra.App.Services
{
    public interface IJwtService
    {
        string GenerateToken(UserEntity user);
    }
}
