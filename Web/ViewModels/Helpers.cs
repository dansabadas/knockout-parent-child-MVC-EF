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
            SalesOrderViewModel salesOrderViewModel = new SalesOrderViewModel()
            {
                SalesOrderId = salesOrder.SalesOrderId,
                CustomerName = salesOrder.CustomerName,
                PONumber = salesOrder.PONumber
            };

            return salesOrderViewModel;
        }


        public static SalesOrder CreateSalesOrderFromSalesOrderViewModel(SalesOrderViewModel salesOrderViewModel)
        {
            SalesOrder salesOrder = new SalesOrder()
            {
                SalesOrderId = salesOrderViewModel.SalesOrderId,
                CustomerName = salesOrderViewModel.CustomerName,
                PONumber = salesOrderViewModel.PONumber
            };

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
                    messageToClient = string.Format("The customer name for this sales order has been updated to {0} in the database.", customerName);
                    break;
            }

            return messageToClient;
        }
    }
}