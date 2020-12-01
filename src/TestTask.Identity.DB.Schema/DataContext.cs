using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Data.SqlClient;
using TestTask.Identity.DAL.Entities;

namespace TestTask.Identity.DB.Schema
{
    public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
                                                 UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
                                                 IdentityUserToken<Guid>>
    {
        readonly string _connectionString = "Server=VLAD-NOUT-HP;Initial Catalog=TestTask.Identity.Dev;Integrated Security=True;MultipleActiveResultSets=True";

        public DataContext() { }

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(new SqlConnection(_connectionString));
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

        }
    }
}
