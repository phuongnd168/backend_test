using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Lib.Data.Sql;

namespace TLS.BHL.Infra.Data.SQL
{
    public interface IBHLDbContext : IDbContext
    {
        DbSet<UserEntity> Users { get; }
        DbSet<ProductEntity> Products { get; }
        DbSet<CategoryEntity> Categories { get; }
        DbSet<OrderEntity> Orders { get; }
        DbSet<ProductCategoryEntity> ProductCategories { get; }
        DbSet<ForgotPasswordEntity> ForgotPassword { get; }
    }
}
