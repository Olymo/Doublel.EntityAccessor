using Doublel.EntityAccessor.test.Entities;
using Doublel.EntityAccessor.Tests.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace Doublel.EntityAccessor.Tests
{
    public class TestDbContext : DbContext, IDisposable
    {
        private readonly DbConnection _connection;

        public TestDbContext()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(_connection).UseLazyLoadingProxies();

        public DbSet<Role> Roles { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
