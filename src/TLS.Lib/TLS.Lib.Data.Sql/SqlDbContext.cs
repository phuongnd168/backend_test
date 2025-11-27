using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Lib.Data.Sql
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions options) : base (options)
        {

        }
    }
}
