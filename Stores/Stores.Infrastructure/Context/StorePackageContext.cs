using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Stores.Domain.Models;
using System;

namespace Stores.Infrastructure.Context
{
    public interface IStorePackageContext
    {
        DbSet<StorePackage> Packages { get; set; }
        DbSet<StoreProduct> Products { get; set; }
    }

    public class StorePackageContext: DbContext, IStorePackageContext
    {
        public StorePackageContext(DbContextOptions<StorePackageContext> options)
            : base(options)
        { }

        public DbSet<StorePackage> Packages { get; set; }
        public DbSet<StoreProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StorePackage>(entity =>
            {
                entity.HasKey(new string[] { "Id" });
                entity.Property(package => package.Id).ValueGeneratedOnAdd();

                entity.HasMany<StoreProduct>()
                    .WithOne(product => product.Package)
                    .HasForeignKey(product => product.PackageId);
            });

            modelBuilder.Entity<StoreProduct>(entity =>
            {
                entity.HasKey(new string[] { "Id" });
                entity.Property(product => product.Id).ValueGeneratedOnAdd();

                entity.HasOne<StorePackage>()
                    .WithMany(package => package.Products)
                    .HasForeignKey(product => product.PackageId);
            });
        }
    }

    public class StorePackageContextFactory : IDesignTimeDbContextFactory<StorePackageContext>
    {
        private readonly string SqlConnectionString;

        public StorePackageContextFactory()
        {
            var dbServerName = Environment.GetEnvironmentVariable("MSSQL_SERVER_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("SA_USER");
            var dbName = Environment.GetEnvironmentVariable("MSSQL_DB_NAME");
            
            SqlConnectionString = $@"Data Source={dbServerName};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword};";
        }

        public StorePackageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StorePackageContext>();

            optionsBuilder.UseSqlServer(SqlConnectionString);

            return new StorePackageContext(optionsBuilder.Options);
        }
    }
}
