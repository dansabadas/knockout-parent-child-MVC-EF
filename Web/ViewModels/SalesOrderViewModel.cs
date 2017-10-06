using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model;

namespace Web.ViewModels
{
  public class SalesOrderViewModel : IObjectWithState
  {
    public SalesOrderViewModel()
    {
      SalesOrderItems = new List<SalesOrderItemViewModel>();
      SalesOrderItemsToDelete = new List<int>();  // if we don't initialize the collections in constructor, the JS-serialization won't work!
    }

    public int SalesOrderId { get; set; }

    [Required(ErrorMessage = "Server: You cannot create a sales order unless you supply the customer's name.")]
    [StringLength(30, ErrorMessage = "Server: Customer names must be 30 characters or shorter.")]
    [CheckScore(3.14)]
    public string CustomerName { get; set; }

    [StringLength(10, ErrorMessage = "Server: PO numbers must be 10 characters or shorter.")]
    public string PONumber { get; set; }

    public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

    public List<int> SalesOrderItemsToDelete { get; set; }

    public string MessageToClient { get; set; }

    public ObjectState ObjectState { get; set; }
  }
}