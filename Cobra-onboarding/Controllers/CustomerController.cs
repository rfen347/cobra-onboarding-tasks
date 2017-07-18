using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cobra_onboarding.Models;

namespace Cobra_onboarding.Controllers
{
    public class CustomerController : Controller
    {
        private CobraContext db = new CobraContext();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customer/Get
        public JsonResult Get()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.People.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,Address1,Address2,City")] Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.People.Add(person);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(person);
        //}

        [HttpPost]
        public ActionResult Create(String name, String add1, String add2, String city)//([Bind(Include = "Id,Name,Address1,Address2,City")] Person person)
        {
                if (ModelState.IsValid)
                {
                    Person person = new Person();

                    person.Name = name;
                    person.Address1 = add1;
                    person.Address2 = add2;
                    person.City = city;

                    db.People.Add(person);

                    db.SaveChanges();
                    return Json(person, JsonRequestBehavior.AllowGet);
                }
            return View();

        }


        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, String name, String add1, String add2, String city)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(person).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(person);
            db.Configuration.ProxyCreationEnabled = false;
            if (db.People.Any(o => o.Id.Equals(id)))
            {
                Person person = db.People.Where(o => o.Id.Equals(id)).First();
                person.Name = name;
                person.Address1 = add1;
                person.Address2 = add2;
                person.City = city;
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return Json(person, JsonRequestBehavior.DenyGet);

            }
            return View();
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }
        //Takes in customer ID, returns entire person object
        // POST: Customer/Delete/5
       [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            var orderheaderlist = db.OrderHeaders.Where(o => o.PersonId.Equals(id));
            foreach (OrderHeader order in orderheaderlist)
            {
                OrderDetail od = db.OrderDetails.Where(o => o.OrderId.Value.Equals(order.OrderId)).First();
                db.OrderDetails.Remove(od);
                db.OrderHeaders.Remove(order);
            }
            Person person = db.People.Where(o => o.Id.Equals(id)).First();
            db.People.Remove(person);
            db.SaveChanges();
            return Json(person, JsonRequestBehavior.DenyGet);
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
