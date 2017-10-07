using Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer
{
  public class SalesOrderItemConfiguration : EntityTypeConfiguration<SalesOrderItem>
  {
    public SalesOrderItemConfiguration()
    {
      Property(soi => soi.ProductCode).HasMaxLength(15).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_SalesOrderItem", 2) { IsUnique = true }));
      Property(soi => soi.Quantity).IsRequired();
      Property(soi => soi.UnitPrice).IsRequired();
      Ignore(soi => soi.ObjectState);

      Property(soi => soi.SalesOrderId).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("AK_SalesOrderItem", 1) { IsUnique = true }));
      Property(so => so.RowVersion).IsRowVersion();
    }
  }
}
