using System.Data.Entity.ModelConfiguration;
using Model;

namespace DataLayer
{
  public class SalesOrderConfiguration : EntityTypeConfiguration<SalesOrder>
  {
    public SalesOrderConfiguration()
    {
      Property(so => so.CustomerName).HasMaxLength(30).IsRequired();
      Property(so => so.PONumber).HasMaxLength(10).IsOptional();
    }
  }
}
