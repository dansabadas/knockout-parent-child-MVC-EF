using System.Collections.Generic;
using Model;

namespace Web.ViewModels
{
  public class SalesOrderViewModel : IObjectWithState
  {
    public SalesOrderViewModel()
    {
      SalesOrderItems = new List<SalesOrderItemViewModel>();
    }

    public int SalesOrderId { get; set; }

    public string CustomerName { get; set; }

    public string PONumber { get; set; }

    public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }

    public string MessageToClient { get; set; }

    public ObjectState ObjectState { get; set; }
  }
}