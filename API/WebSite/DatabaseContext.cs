﻿using Microsoft.EntityFrameworkCore;
using WebSite.Domains;

namespace WebSite
{
    public class DatabaseContext : DbContext
    {
        public DbSet<NewsDomain> NewsDomain { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }
    }
}