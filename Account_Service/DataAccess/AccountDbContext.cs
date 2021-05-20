using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account_Service.Entities;
using Account_Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Account_Service.DataAccess
{
    public class AccountDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }

        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false).Build();

        //    string connectionStr = configuration["ConnectionStrings:DefaultConnection"];
        //    optionsBuilder.UseSqlServer();
        //}
    }
}
