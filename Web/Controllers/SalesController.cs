﻿using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DataLayer;
using Model;
using Web.ViewModels;
using System.Data.Entity.Infrastructure;

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

    [HandleModelStateException]
    public async Task<JsonResult> Save(SalesOrderViewModel salesOrderViewModel)
    {
      if (!ModelState.IsValid)
      {
        throw new ModelStateException(ModelState);  // if this is thrown => HandleModelStateException Attribute above will intercept the exception
      }

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
      string messageToClient = string.Empty;
      try
      {
        await _salesContext.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        messageToClient = "Someone else have modified this sales order since you retrieved it.  Your changes have not been applied.  What you see now are the current values in the database.";
      }
      catch (System.Exception ex)
      {
        throw new ModelStateException(ex);
      }

      if (salesOrder.ObjectState == ObjectState.Deleted)
        return Json(new { newLocation = "/Sales/Index/" });

      if (messageToClient.Trim().Length == 0)
        messageToClient = ViewModels.Helpers.GetMessageToClient(salesOrderViewModel.ObjectState, salesOrder.CustomerName);

      salesOrderViewModel.SalesOrderId = salesOrder.SalesOrderId;
      _salesContext.Dispose();
      _salesContext = new SalesContext();
      salesOrder = _salesContext.SalesOrders.Find(salesOrderViewModel.SalesOrderId);

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
