using Cobra_onboarding.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cobra_onboarding.Controllers
{
    public class OrdersController : Controller
    {
        private CobraContext db = new CobraContext();
        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get()
        {
            var query = from oh in db.OrderHeaders
                        join p in db.People on oh.PersonId equals p.Id
                        join od in db.OrderDetails on oh.OrderId equals od.OrderId
                        join pr in db.Products on od.ProductId equals pr.Id
                        select new
                        {
                            Id = od.Id,
                            OrderId = oh.OrderId,
                            Date = oh.OrderDate,
                            CustomerId = p.Id,
                            CustomerName = p.Name,
                            ProductId = pr.Id,
                            Product = pr.ProductName,
                            Price = pr.Price
                        };

           
            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }
        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        public JsonResult Create(DateTime date, int customerid, int productid)
        {
            OrderHeader orderheader = new OrderHeader();
            orderheader.PersonId = customerid;
            orderheader.OrderDate = date;
            db.OrderHeaders.Add(orderheader);
            db.SaveChanges();
            OrderDetail orderdetail = new OrderDetail();
            orderdetail.OrderId = orderheader.OrderId;
            orderdetail.ProductId = productid;
            db.OrderDetails.Add(orderdetail);
            db.SaveChanges();

            //return View();

            return Json(new
            {
                Id = orderdetail.Id,
                OrderId = orderdetail.OrderId,
                Date = orderheader.OrderDate,
                CustomerId = orderheader.PersonId,
                CustomerName = db.People.Find(orderheader.PersonId).Name,
                ProductId = orderdetail.ProductId,
                Product = db.Products.Find(orderdetail.ProductId).ProductName,
                Price = db.Products.Find(orderdetail.ProductId).Price
            }, JsonRequestBehavior.DenyGet);

        }

        // GET: Orders/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, int orderid, DateTime date, int customerid, int productid)
        {
       
                
                OrderHeader orderheader = db.OrderHeaders.Where(o => o.OrderId.Equals(orderid)).First();
                orderheader.OrderId = orderid;
                orderheader.PersonId = customerid;
                db.Entry(orderheader).State = EntityState.Modified;
                db.SaveChanges();
                    OrderDetail orderdetail = db.OrderDetails.Where(o => o.Id.Equals(id)).First();
                    orderdetail.OrderId = orderid;
                    orderdetail.ProductId = productid;
                    db.Entry(orderdetail).State = EntityState.Modified;
                    db.SaveChanges();
      
 
            return Json(new
            {
                Id = orderdetail.Id,
                OrderId = orderdetail.OrderId,
                Date = orderheader.OrderDate,
                CustomerId = orderheader.PersonId,
                CustomerName = db.People.Find(orderheader.PersonId).Name,
                ProductId = orderdetail.ProductId,
                Product = db.Products.Find(orderdetail.ProductId).ProductName,
                Price = db.Products.Find(orderdetail.ProductId).Price
            }, JsonRequestBehavior.DenyGet);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int orderId)
        {
            
                OrderDetail orderDetails = db.OrderDetails.Where(o => o.Id.Equals(id)).First();
                db.OrderDetails.Remove(orderDetails);

                OrderHeader orderHeader = db.OrderHeaders.Where(o => o.OrderId.Equals(orderId)).First();
                db.OrderHeaders.Remove(orderHeader);
                
                db.SaveChanges();

            return Json(orderDetails, JsonRequestBehavior.DenyGet);
            }
            
        }
    }

