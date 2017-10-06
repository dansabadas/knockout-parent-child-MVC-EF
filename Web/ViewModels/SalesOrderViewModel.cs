using System.Collections.Generic;
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

    public string CustomerName { get; set; }

    public string PONumber { get; set; }

    public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

    public List<int> SalesOrderItemsToDelete { get; set; }

    public string MessageToClient { get; set; }

    public ObjectState ObjectState { get; set; }
  }
}