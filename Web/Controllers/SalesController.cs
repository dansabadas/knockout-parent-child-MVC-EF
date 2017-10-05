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
      return View();
    }

    // POST: Sales/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
    {
      if (ModelState.IsValid)
      {
        _salesContext.SalesOrders.Add(salesOrder);
        await _salesContext.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(salesOrder);
    }

    // GET: Sales/Edit/5
    public async Task<ActionResult> Edit(int? id)
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
      return View(salesOrder);
    }

    // POST: Sales/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "SalesOrderId,CustomerName,PONumber")] SalesOrder salesOrder)
    {
      if (ModelState.IsValid)
      {
        _salesContext.Entry(salesOrder).State = EntityState.Modified;
        await _salesContext.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(salesOrder);
    }

    // GET: Sales/Delete/5
    public async Task<ActionResult> Delete(int? id)
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
      return View(salesOrder);
    }

    // POST: Sales/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      SalesOrder salesOrder = await _salesContext.SalesOrders.FindAsync(id);
      _salesContext.SalesOrders.Remove(salesOrder);
      await _salesContext.SaveChangesAsync();
      return RedirectToAction("Index");
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
