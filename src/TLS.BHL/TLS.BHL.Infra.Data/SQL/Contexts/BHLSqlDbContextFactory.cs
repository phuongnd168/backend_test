using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TLS.BHL.Infra.Data.SQL.Contexts
{
    public class BHLSqlDbContextFactory : IDesignTimeDbContextFactory<BHLSqlDbContext>
    {
        public BHLSqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BHLSqlDbContext>();

             optionsBuilder.UseSqlServer("Data Source=DESKTOP-6FQI9UO\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
 );

            return new BHLSqlDbContext(optionsBuilder.Options);
        }
    }
}
