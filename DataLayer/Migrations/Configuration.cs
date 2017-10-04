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
        new SalesOrder {CustomerName = "Peter", PONumber = "ALO"});
      context.SaveChanges();
    }
  }
}
