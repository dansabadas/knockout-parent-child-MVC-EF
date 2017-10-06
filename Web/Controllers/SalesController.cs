using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DataLayer;
using Model;
using Web.ViewModels;

namespace Web.Controllers
{
  public class SalesController : Controller
  {
    private SalesContext _salesContext;

    public SalesController()
    {
      _salesContext = new SalesContext();
    }

    // GET: Sales
    public async Task<ActionResult> Index()
    {
      return View(await _salesContext.SalesOrders.ToListAsync());
    }

    // GET: Sales/Details/5
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      SalesOrder salesOrder = await _salesContext.SalesOrders.FindAsync(id);
      if (salesOrder == null)
      {
        return HttpNotFound();
      }

      var salesOrderViewModel = ViewModels.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
      salesOrderViewModel.MessageToClient = "I originated from the viewmodel, rather than the model.";

      return View(salesOrderViewModel);
    }

    // GET: Sales/Create
    public ActionResult Create()
    {
      var salesOrderViewModel = new SalesOrderViewModel {ObjectState = ObjectState.Added };
      return View(salesOrderViewModel);
    }

    // GET: Sales/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      var salesOrder = await _salesContext.SalesOrders.FindAsync(id);
      if (salesOrder == null)
      {
        return HttpNotFound();
      }

      var salesOrderViewModel = ViewModels.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
      salesOrderViewModel.MessageToClient = string.Format("The original value of Customer Name is {0}.", salesOrderViewModel.CustomerName);

      return View(salesOrderViewModel);
    }

    // GET: Sales/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      var salesOrder = await _salesContext.SalesOrders.FindAsync(id);
      if (salesOrder == null)
      {
        return HttpNotFound();
      }

      var salesOrderViewModel = ViewModels.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
      salesOrderViewModel.MessageToClient = "You are about to permanently delete this sales order.";
      salesOrderViewModel.ObjectState = ObjectState.Deleted;

      return View(salesOrderViewModel);
    }

    public async Task<JsonResult> Save(SalesOrderViewModel salesOrderViewModel)
    {
      var salesOrder = ViewModels.Helpers.CreateSalesOrderFromSalesOrderViewModel(salesOrderViewModel);

      _salesContext.SalesOrders.Attach(salesOrder);

      if (salesOrder.ObjectState == ObjectState.Deleted)  // cascade delete if the parent order is to be deleted
      {
        foreach (SalesOrderItemViewModel salesOrderItemViewModel in salesOrderViewModel.SalesOrderItems)
        {
          SalesOrderItem salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemViewModel.SalesOrderItemId);
          if (salesOrderItem != null)
            salesOrderItem.ObjectState = ObjectState.Deleted;
        }
      }
      else
      {
        foreach (int salesOrderItemId in salesOrderViewModel.SalesOrderItemsToDelete) // if not => see if any children need to be deleted
        {
          SalesOrderItem salesOrderItem = _salesContext.SalesOrderItems.Find(salesOrderItemId);
          if (salesOrderItem != null)
            salesOrderItem.ObjectState = ObjectState.Deleted;
        }
      }

      _salesContext.ApplyStateChanges();
      await _salesContext.SaveChangesAsync();

      if (salesOrder.ObjectState == ObjectState.Deleted)
        return Json(new { newLocation = "/Sales/Index/" });

      string messageToClient = ViewModels.Helpers.GetMessageToClient(salesOrderViewModel.ObjectState, salesOrder.CustomerName);
      salesOrderViewModel = ViewModels.Helpers.CreateSalesOrderViewModelFromSalesOrder(salesOrder);
      salesOrderViewModel.MessageToClient = messageToClient;

      return Json(new { salesOrderViewModel }); // watch out that the client AJAX response gets this as data.salesOrderViewModel
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _salesContext.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
