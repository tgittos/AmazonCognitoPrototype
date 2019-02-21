using System;
using AmazonCognitoSpike.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazonCognitoSpike.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=AmazonCognitoSpike.db");
        }
    }
}
