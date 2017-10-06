using Model;

namespace Web.ViewModels
{
  /// <summary>
  /// this could be replaced by AutoMapper
  /// </summary>
  public static class Helpers
  {
    public static SalesOrderViewModel CreateSalesOrderViewModelFromSalesOrder(SalesOrder salesOrder)
    {
      SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel
      {
        SalesOrderId = salesOrder.SalesOrderId,
        CustomerName = salesOrder.CustomerName,
        PONumber = salesOrder.PONumber,
        ObjectState = ObjectState.Unchanged
      };

      foreach (SalesOrderItem salesOrderItem in salesOrder.SalesOrderItems)
      {
        SalesOrderItemViewModel salesOrderItemViewModel = new SalesOrderItemViewModel
        {
          SalesOrderItemId = salesOrderItem.SalesOrderItemId,
          ProductCode = salesOrderItem.ProductCode,
          Quantity = salesOrderItem.Quantity,
          UnitPrice = salesOrderItem.UnitPrice,
          ObjectState = ObjectState.Unchanged,
          SalesOrderId = salesOrder.SalesOrderId
        };

        salesOrderViewModel.SalesOrderItems.Add(salesOrderItemViewModel);
      }
      return salesOrderViewModel;
    }


    public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
    {
      SalesOrder salesOrder = new SalesOrder
      {
        SalesOrderId = salesOrderViewModel.SalesOrderId,
        CustomerName = salesOrderViewModel.CustomerName,
        PONumber = salesOrderViewModel.PONumber,
        ObjectState = salesOrderViewModel.ObjectState
      };

      int temporarySalesOrderItemId = -1;

      foreach (SalesOrderItemViewModel salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
      {
        SalesOrderItem salesOrderItem = new SalesOrderItem
        {
          ProductCode = salesOrderItemViewModel.ProductCode,
          Quantity = salesOrderItemViewModel.Quantity,
          UnitPrice = salesOrderItemViewModel.UnitPrice,
          ObjectState = salesOrderItemViewModel.ObjectState
        };


        if (salesOrderItemViewModel.ObjectState != ObjectState.Added)
          salesOrderItem.SalesOrderItemId = salesOrderItemViewModel.SalesOrderItemId;
        else
        {
          salesOrderItem.SalesOrderItemId = temporarySalesOrderItemId;
          temporarySalesOrderItemId--;  // needed for EF to have unique entities when attached to context
        }

        salesOrderItem.SalesOrderId = salesOrderViewModel.SalesOrderId;

        salesOrder.SalesOrderItems.Add(salesOrderItem);
      }

      return salesOrder;
    }


    public static string GetMessageToClient(ObjectState objectState, string customerName)
    {
      string messageToClient = string.Empty;

      switch (objectState)
      {
        case ObjectState.Added:
          messageToClient = string.Format("A sales order for {0} has been added to the database.", customerName);
          break;

        case ObjectState.Modified:
          messageToClient =
            string.Format("The customer name for this sales order has been updated to {0} in the database.",
              customerName);
          break;
      }

      return messageToClient;
    }
  }
}