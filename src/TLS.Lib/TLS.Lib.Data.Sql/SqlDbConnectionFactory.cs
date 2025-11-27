using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Data;

namespace TLS.Lib.Data.SQL
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Open(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public async Task<IDbConnection> OpenAsync(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
