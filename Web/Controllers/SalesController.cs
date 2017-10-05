using System.Data.Entity;
using System.Linq;
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

      var salesOrderViewModel = new SalesOrderViewModel
      {
        SalesOrderId = salesOrder.SalesOrderId,
        CustomerName = salesOrder.CustomerName,
        PONumber = salesOrder.PONumber,
        MessageToClient = "I originated from the viewmodel, rather than the model."
      }; // mapping happens here

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

      var salesOrderViewModel = new SalesOrderViewModel
      {
        SalesOrderId = salesOrder.SalesOrderId,
        CustomerName = salesOrder.CustomerName,
        PONumber = salesOrder.PONumber,
        ObjectState = ObjectState.Unchanged
      };
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

      var salesOrderViewModel = new SalesOrderViewModel
      {
        SalesOrderId = salesOrder.SalesOrderId,
        CustomerName = salesOrder.CustomerName,
        PONumber = salesOrder.PONumber,
        MessageToClient = "You are about to permanently delete this sales order.",
        ObjectState = ObjectState.Deleted
      };

      return View(salesOrderViewModel);
    }

    public async Task<JsonResult> Save(SalesOrderViewModel salesOrderViewModel)
    {
      var salesOrder = new SalesOrder
      {
        SalesOrderId = salesOrderViewModel.SalesOrderId,
        CustomerName = salesOrderViewModel.CustomerName,
        PONumber = salesOrderViewModel.PONumber,
        ObjectState = salesOrderViewModel.ObjectState
      };

      _salesContext.SalesOrders.Attach(salesOrder);
      _salesContext.ChangeTracker.Entries<IObjectWithState>().Single().State = Helpers.ConvertState(salesOrder.ObjectState);
      await _salesContext.SaveChangesAsync();

      if (salesOrder.ObjectState == ObjectState.Deleted)
        return Json(new { newLocation = "/Sales/Index/" });

      switch (salesOrderViewModel.ObjectState)
      {
        case ObjectState.Added:
          salesOrderViewModel.MessageToClient = string.Format("A sales order for {0} has been added to the database.", salesOrder.CustomerName);
          break;

        case ObjectState.Modified:
          salesOrderViewModel.MessageToClient = string.Format("The customer name for this sales order has been updated to {0} in the database.", salesOrder.CustomerName);
          break;
      }

      salesOrderViewModel.SalesOrderId = salesOrder.SalesOrderId;
      salesOrderViewModel.ObjectState = ObjectState.Unchanged;

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
