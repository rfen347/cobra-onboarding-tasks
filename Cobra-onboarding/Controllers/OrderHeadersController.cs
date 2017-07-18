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
    public class OrderHeadersController : Controller
    {
        private CobraContext db = new CobraContext();

        // GET: OrderHeaders
        public ActionResult Index()
        {
            var orderHeaders = db.OrderHeaders.Include(o => o.Person);
            return View(orderHeaders.ToList());
        }

        // GET: OrderHeaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderHeader orderHeader = db.OrderHeaders.Find(id);
            if (orderHeader == null)
            {
                return HttpNotFound();
            }
            return View(orderHeader);
        }

        // GET: OrderHeaders/Create
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name");
            return View();
        }

        // POST: OrderHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderDate,PersonId")] OrderHeader orderHeader)
        {
            if (ModelState.IsValid)
            {
                db.OrderHeaders.Add(orderHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", orderHeader.PersonId);
            return View(orderHeader);
        }

        // GET: OrderHeaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderHeader orderHeader = db.OrderHeaders.Find(id);
            if (orderHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", orderHeader.PersonId);
            return View(orderHeader);
        }

        // POST: OrderHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderDate,PersonId")] OrderHeader orderHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", orderHeader.PersonId);
            return View(orderHeader);
        }

        // GET: OrderHeaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderHeader orderHeader = db.OrderHeaders.Find(id);
            if (orderHeader == null)
            {
                return HttpNotFound();
            }
            return View(orderHeader);
        }

        // POST: OrderHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderHeader orderHeader = db.OrderHeaders.Find(id);
            db.OrderHeaders.Remove(orderHeader);
            db.SaveChanges();
            return RedirectToAction("Index");
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
