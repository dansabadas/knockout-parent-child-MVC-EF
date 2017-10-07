using System.Data.Entity.Migrations;
using Model;

namespace DataLayer.Migrations
{
  internal sealed class Configuration : DbMigrationsConfiguration<SalesContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
      AutomaticMigrationDataLossAllowed = true;
    }

    protected override void Seed(SalesContext context)
    {
      context.SalesOrders.AddOrUpdate(
        so => so.CustomerName,
        new SalesOrder {CustomerName = "Dan", PONumber = "1235"},
        new SalesOrder {CustomerName = "Gab", PONumber = "4567"},
        new SalesOrder {CustomerName = "Peter", PONumber = "ALO"},
        new SalesOrder
        {
          CustomerName = "Adam",
          PONumber = "9876",
          SalesOrderItems =
          {
            new SalesOrderItem {ProductCode = "ABC", Quantity = 10, UnitPrice = 1.23m},
            new SalesOrderItem {ProductCode = "XYZ", Quantity = 7, UnitPrice = 14.57m},
            new SalesOrderItem {ProductCode = "SAMPLE", Quantity = 3, UnitPrice = 15.00m}
          }
        });
      context.SaveChanges();
    }
  }
}
