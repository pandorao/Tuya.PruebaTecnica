﻿using Microsoft.EntityFrameworkCore;
using Tuya.PruebaTecnica.Models.Models;

namespace Tuya.PruebaTecnica.ProductsService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
    }
}
