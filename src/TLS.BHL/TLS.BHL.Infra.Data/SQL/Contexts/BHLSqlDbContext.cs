using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Lib.Data.Sql;
using static Dapper.SqlMapper;

namespace TLS.BHL.Infra.Data.SQL.Contexts
{
    public class BHLSqlDbContext : SqlDbContext, IBHLDbContext
    {
        public BHLSqlDbContext(DbContextOptions<BHLSqlDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; } = default!;

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }

        public DbSet<ForgotPasswordEntity> ForgotPassword { get; set; }
        public int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ValidateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void ValidateEntities()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(entry.Entity);

                    if (!Validator.TryValidateObject(entry.Entity, validationContext, validationResults, true))
                    {
                        throw new ValidationException("Entity validation failed.");
                    }
                }
            }
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("data source=192.168.222.13\\sql2014;initial catalog=TestNetCore;persist security info=True;user id=sa;password=dsvn@123456;multipleactiveresultsets=True;Max Pool Size=1000;Connect Timeout=600;TrustServerCertificate=true;");
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<UserEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn(1, 1);
                entity.Property(x => x.FullName).IsRequired().IsUnicode().HasMaxLength(250);
                entity.Property(x => x.Updated_by).IsUnicode().HasMaxLength(50);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<RoleEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn(1, 1);
                entity.Property(x => x.RoleName).IsRequired().IsUnicode().HasMaxLength(250);
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });

            builder.Entity<UserRoleEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn(1, 1);
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.RoleId).IsRequired();
                entity.HasOne(x => x.Role).WithMany(y => y.RoleUsers).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Role_Users");
                entity.HasOne(x => x.User).WithMany(y => y.UserRoles).HasForeignKey(y => y.UserId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User_Roles");
            });

            builder.Entity<PermissionEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn(1, 1);
                entity.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(250);
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<CategoryEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.Property(x => x.NameVi).IsRequired().HasMaxLength(250);
                entity.Property(x => x.NameEn).IsRequired().HasMaxLength(250);
                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });

            builder.Entity<ProductEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.Property(x => x.NameVi).IsRequired().HasMaxLength(250);
                entity.Property(x => x.NameEn).IsRequired().HasMaxLength(250);
                entity.Property(x => x.Img).IsRequired().HasMaxLength(250);
                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<OrderEntity>(entity =>
            {
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.Property(x => x.Products).IsRequired();

                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.statusId).HasDefaultValue(0);
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<ProductCategoryEntity>(entity =>
            {
                entity.Property(x => x.ProductsId);
                entity.Property(x => x.CategoriesId);
                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<ForgotPasswordEntity>(entity =>
            {
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.Otp).IsRequired().HasMaxLength(6);
                entity.Property(x => x.ExpiredOtpAt).IsRequired();
                entity.Property(x => x.Deleted_by).HasDefaultValue("phuong");
                entity.Property(x => x.Created_by).HasDefaultValue("phuong");
                entity.Property(x => x.Updated_by).HasDefaultValue("phuong");
                entity.Property(x => x.Deleted).HasDefaultValue(false);
            });
            builder.Entity<ForgotPasswordEntity>()
                .HasOne(u => u.User)
                .WithOne(p => p.ForgotPassword)
                .HasForeignKey<ForgotPasswordEntity>(p => p.UserId);

            //builder.Entity<ProductEntity>()
            //.HasMany(p => p.Categories)
            //.WithMany(c => c.Products)
            //.UsingEntity(j => j.ToTable("ProductCategories"));
            builder.Entity<OrderEntity>()
            .HasOne(o => o.User)          // 1 Order có 1 User
            .WithMany(u => u.Orders)      // 1 User có nhiều Orders
            .HasForeignKey(o => o.UserId) // FK nằm ở Order
            .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderEntity>()
           .HasOne(o => o.Status)
           .WithMany(u => u.Orders)
           .HasForeignKey(o => o.statusId)
           .OnDelete(DeleteBehavior.Cascade);
         
        }
    }
}
