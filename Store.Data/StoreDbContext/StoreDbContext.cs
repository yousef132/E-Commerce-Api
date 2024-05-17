﻿using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.StoreDbContext
{
    public class StoreDbContext:DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> option):base(option)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            // apply all configurations 
		    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
