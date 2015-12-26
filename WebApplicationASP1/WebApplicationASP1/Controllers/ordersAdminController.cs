using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationASP1;

namespace WebApplicationASP1.Controllers
{
    public class ordersAdminController : Controller
    {
        private Db2Entities db = new Db2Entities();

        // GET: ordersAdmin
        public ActionResult Index()
        {
            var orders = db.orders.Include(o => o.drink);
            return View(orders.ToList());
        }

        // GET: ordersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: ordersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.IdDrink = new SelectList(db.drinks, "IdDrink", "DrinkName");
            return View();
        }

        // POST: ordersAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOrder,Name,Amount,IdDrink")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDrink = new SelectList(db.drinks, "IdDrink", "DrinkName", order.IdDrink);
            return View(order);
        }

        // GET: ordersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdDrink = new SelectList(db.drinks, "IdDrink", "DrinkName", order.IdDrink);
            return View(order);
        }

        // POST: ordersAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrder,Name,Amount,IdDrink")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDrink = new SelectList(db.drinks, "IdDrink", "DrinkName", order.IdDrink);
            return View(order);
        }







        // GET: ordersAdmin/Accept/5
        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }



        // POST: OrderAdmin/Accept/5
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptConfirmed(int id)
        {
            order order = db.orders.Find(id);

            using (Db2Entities dbContext = new Db2Entities())
            {
                drink drink = dbContext.drinks.SingleOrDefault(x => x.IdDrink == order.IdDrink);
                
                if (drink.Amount >= order.Amount)
                {
                    drink.Amount = drink.Amount - order.Amount;
                    dbContext.SaveChanges();
                    db.orders.Remove(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("StockError");
                }
                
            }
        }

        public ActionResult StockError()
        {
            return View();
        }






        //// POST: ordersAdmin/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    order order = db.orders.Find(id);
        //    db.orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}












        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
