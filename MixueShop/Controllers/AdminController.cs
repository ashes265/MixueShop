using Microsoft.AspNetCore.Mvc;
using MixueShop.Models;
using System.Linq;
using MixueShop.Logic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MixueShop.Helpers;
using System;
using System.Globalization;
using MixueShop.DataAccess;
namespace MixueShop.Controllers
{
    public class AdminController : Controller
    {
        MixueProjectContext db = new MixueProjectContext();
        DBContext dbContext = new DBContext();
        public IActionResult ListProducts(int page, string search, int sort)
        {
            ViewBag.Category = db.Categories.ToList();
            //setting page
            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 7;
            ProductManage pMNG = new ProductManage();
            ViewBag.products = pMNG.GetProducts((page - 1) * pageSize + 1, pageSize, search, sort);

            int amountPro = pMNG.getNumberOfProducts(search, sort);

            int TotalPage = amountPro / pageSize;
            if (amountPro % pageSize != 0)
            {
                TotalPage++;
            }
            ViewData["TotalPage"] = TotalPage;
            ViewData["CurrentPage"] = page;
            ViewBag.key = search;
            ViewBag.sortSelected = sort;
            //ViewBag.cateSelected = cate;
            ViewBag.pageSelected = page;

            //account
            ViewBag.Error = HttpContext.Session.GetString("error");
            ViewBag.Name = HttpContext.Session.GetString("name");
            ViewBag.Role = HttpContext.Session.GetInt32("role");
            return View();
        }
        public IActionResult ListOrders(int page, string from, string to)
        {
            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 4;
            OrderManage oMNG = new OrderManage();
            if (from != null && to != null)
            {
                DateTime forFrom = DateTime.Parse(from);
                DateTime forTo = DateTime.Parse(to);
                int x = DateTime.Compare(forFrom, forTo);
                if (x > 0)
                {
                    ViewBag.mess = "From date need more than to date";
                    //account
                    HttpContext.Session.Remove("error");
                    ViewBag.Name = HttpContext.Session.GetString("name");
                    ViewBag.Role = HttpContext.Session.GetInt32("role");
                    return View();
                }
                else
                {
                    ViewBag.orders = dbContext.getAllOrdersAmin();
                    ViewBag.orders = oMNG.getAllOrders((page - 1) * pageSize + 1, pageSize, from, to);
                    int amountO = oMNG.getNumberOrder(from, to);

                    int TotalPage = amountO / pageSize;
                    if (amountO % pageSize != 0)
                    {
                        TotalPage++;
                    }
                    ViewData["TotalPage"] = TotalPage;
                    ViewData["CurrentPage"] = page;
                    ViewBag.pageSelected = page;
                    //account
                    HttpContext.Session.Remove("error");
                    ViewBag.Name = HttpContext.Session.GetString("name");
                    ViewBag.Role = HttpContext.Session.GetInt32("role");
                    return View();
                }
            }
            else
            {
                ViewBag.orders = oMNG.getAllOrdersAmin((page - 1) * pageSize + 1, pageSize, from, to);
                int amountO = oMNG.getNumberOrder(from, to);

                int TotalPage = amountO / pageSize;
                if (amountO % pageSize != 0)
                {
                    TotalPage++;
                }
                ViewData["TotalPage"] = TotalPage;
                ViewData["CurrentPage"] = page;
                ViewBag.pageSelected = page;
                //account
                HttpContext.Session.Remove("error");
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Role = HttpContext.Session.GetInt32("role");
                return View();
            }


        }

        public IActionResult OrderDetail(int OrderId)
        {
            HttpContext.Session.Remove("error");
            var orderdetail = (from b in db.OrderDetails
                              select new OrderDetail
                              {
                                  OrderId = b.OrderId,
                                  DetailOrderId = b.DetailOrderId,
                                  //OdAmount = b.Price * b.Quantity,
                                  Price = b.Price,
                                  Quantity = b.Quantity,
                                  ProductId = b.ProductId,
                              }).ToList();
            List<OrderDetail> listOrderDetail = new List<OrderDetail>();
            foreach (var item in orderdetail)
            {
                if (item.OrderId == OrderId)
                {
                    listOrderDetail.Add(item);
                }
            }

            ViewBag.OrderDetails = listOrderDetail;
            ViewBag.Products = db.Products.ToList();
            return View(listOrderDetail);
        }

        // crud san pham
        public IActionResult Add()
        {
            HttpContext.Session.Remove("error");
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product product)
        {

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }
        [HttpGet("Admin/Update/{id}")]
        public IActionResult Update(int id)
        {
            HttpContext.Session.Remove("error");
            ViewBag.Categories = db.Categories.ToList();
            var product = db.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(Product product)
        {
            db.Products.Update(product);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }

        public IActionResult Delete(int id)
        {
            HttpContext.Session.Remove("error");
            var orderDetails = db.OrderDetails.ToList();
            int check = 0;
            foreach (var order in orderDetails)
            {
                if (order.ProductId == id)
                {
                    HttpContext.Session.SetString("error", "San pham da ton tai trong Order Detail");
                    return RedirectToAction("ListProducts");
                }
            }
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }
        public IActionResult OverView()
        {
            ViewBag.totalOrder = db.Orders.Count();
            //ViewBag.totalEarned = db.Orders.Sum(b => b.Amount);
            ProductManage mng = new ProductManage();
            //ViewBag.products = mng.getTopProduct().Take(3);
            DBContext dBContext = new DBContext();
            ViewBag.products = dBContext.getTopPro().Take(3);
            return View();
        }
    }


}
