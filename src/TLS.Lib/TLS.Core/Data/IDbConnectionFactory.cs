using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Open(string connectionString);
        Task<IDbConnection> OpenAsync(string connectionString);
    }
}
