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
    public class drinksAdminController : Controller
    {
        private Db2Entities db = new Db2Entities();

        // GET: drinksAdmin
        public ActionResult Index()
        {
            return View(db.drinks.ToList());
            
        }





        public ActionResult htmlLink()
        {
            return Redirect("http://www.example.com"); 
        }


        



        public ActionResult Login()
        {
            return View();
            //return RedirectToAction("DrinksAdmin", "DrinksAdmin");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (Db2Entities dc = new Db2Entities())
                {
                    var v = dc.Users.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.UserId.ToString();
                        Session["LogedUserFullname"] = v.FullName.ToString();
                        return RedirectToAction("AfterLogin");
                    }
                }
            }
            return View(u);
        }




        public ActionResult AfterLogin()
        {
            if (Session["LogedUserID"] != null)
            {
                //return View();
                return RedirectToAction("Index", "drinksAdmin");
            }
            else
            {
                return ViewBag.Message = "Oh no, database with users is empty!";
            }
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }








        // GET: drinksAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drink drink = db.drinks.Find(id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        // GET: drinksAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: drinksAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDrink,DrinkName,Description,Price,Amount")] drink drink)
        {
            if (ModelState.IsValid)
            {
                db.drinks.Add(drink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drink);
        }

        // GET: drinksAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drink drink = db.drinks.Find(id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        // POST: drinksAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDrink,DrinkName,Description,Price,Amount")] drink drink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drink);
        }

        // GET: drinksAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drink drink = db.drinks.Find(id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        //// POST: drinksAdmin/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    drink drink = db.drinks.Find(id);
            
        //    db.drinks.Remove(drink);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}



        // POST: drinksAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            drink drink = db.drinks.Find(id);

            using (Db2Entities dbContext = new Db2Entities())
            {
                order order = dbContext.orders.SingleOrDefault(x => x.IdDrink == drink.IdDrink );

                if (order != null)
                {
                    return RedirectToAction("OrderError");
                }
                else
                {
                    db.drinks.Remove(drink);
                    db.SaveChanges();
                    return RedirectToAction("Index");                 
                }
            }
        }


        public ActionResult OrderError()
        {
            return View();
        }




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
