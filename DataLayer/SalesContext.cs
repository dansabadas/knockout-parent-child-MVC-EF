﻿using System.Data.Entity;
using Model;

namespace DataLayer
{
  public class SalesContext : DbContext
  {
    public SalesContext() : base("DefaultConnection")
    {
    }


    public DbSet<SalesOrder> SalesOrders { get; set; }

    public DbSet<SalesOrderItem> SalesOrderItems { get; set; }


    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new SalesOrderConfiguration());
      modelBuilder.Configurations.Add(new SalesOrderItemConfiguration());
    }
  }
}
