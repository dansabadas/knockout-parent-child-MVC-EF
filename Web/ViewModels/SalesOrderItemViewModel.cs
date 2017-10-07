using System.ComponentModel.DataAnnotations;
using Model;

namespace Web.ViewModels
{
  public class SalesOrderItemViewModel : IObjectWithState
  {
    public int SalesOrderItemId { get; set; }

    [Required(ErrorMessage = "Server: The product code is required for a sales order item.")]
    [StringLength(15, ErrorMessage = "Server: Product codes are always 15 characters or shorter.")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Server: Product codes consist of letters only.")]
    public string ProductCode { get; set; }

    [Required(ErrorMessage = "Server: You cannot create a sales order item unless you supply the quantity.")]
    [Range(1, 1000000, ErrorMessage = "Server: Quantity must be between 1 and 1,000,000.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Server: Unit price is a required field.")]
    [Range(0, 100000, ErrorMessage = "Server: Unit price must be between zero and 100,000.")]
    public decimal UnitPrice { get; set; }

    public int SalesOrderId { get; set; }

    public ObjectState ObjectState { get; set; }

    public byte[] RowVersion { get; set; }
  }
}