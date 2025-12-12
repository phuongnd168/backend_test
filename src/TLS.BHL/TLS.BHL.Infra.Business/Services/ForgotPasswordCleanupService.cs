using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.Data.SQL.Contexts;

namespace TLS.BHL.Infra.Business.Services
{
    public class ForgotPasswordCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ForgotPasswordCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<BHLSqlDbContext>();

                    var expiredItems = db.ForgotPassword
                        .Where(x => x.ExpiredOtpAt < DateTime.Now && x.ExpiredResetTokenAt <DateTime.Now);

                    db.RemoveRange(expiredItems);
                    await db.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(5)); 
            }
        }
    }

}
