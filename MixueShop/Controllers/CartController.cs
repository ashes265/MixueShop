 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MixueShop.Helpers;
using MixueShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MixueShop.Controllers
{
    public class CartController : Controller
    {
        MixueProjectContext db = new MixueProjectContext();
     
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        public IActionResult Index()

        {
            var myCart = Carts;
            double Amount = 0.0;
            foreach (var a in myCart)
            {
                Amount += a.TotalMoney;
            }
            ViewBag.Amount = Amount;
            ViewBag.carts = Carts;
            return RedirectToAction("ViewProduct", "Home");
        }
        public IActionResult AddToCart(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                var product = db.Products.SingleOrDefault(p => p.ProductId == id);
                if (product.Discount != null)
                {
                    item = new CartItem
                    {
                        Id = id,
                        Name = product.ProductName,
                        Price = (double)(product.ProductPrice*(1-product.Discount)),
                        Quantity = 1
                    };
                }
                else
                {
                    item = new CartItem
                    {
                        Id = id,
                        Name = product.ProductName,
                        Price = product.ProductPrice,
                        Quantity = 1
                    };
                }
                
                myCart.Add(item);
            }
            else
            {
                item.Quantity++;
            }
            HttpContext.Session.Set("GioHang", myCart);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveItem(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.Id == id);
            myCart.Remove(item);
            HttpContext.Session.Set("GioHang", myCart);
            return RedirectToAction("Index");
        }
        public IActionResult CheckoutCart()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double Amount = 0.0;
            foreach (var a in cart)
            {
                Amount += a.TotalMoney;
            }
            try
            {
                Order o = new Order();
                o.OrderDate = DateTime.Now;
                //o.Amount = Amount;
                db.Add(o);
                db.SaveChanges();
                foreach (var item in cart)
                {
                    OrderDetail od = new OrderDetail();
                    od.OrderId = o.OrderId;
                    od.ProductId = item.Id;
                    od.Price = item.Price;
                    od.Quantity = item.Quantity;
                    //od.OdAmount = item.TotalMoney;
                    db.Add(od);
                }
                db.SaveChanges();
                HttpContext.Session.Remove("GioHang");
                return RedirectToAction("ViewProduct", "Home", "");

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public IActionResult Cancel()
        {
            HttpContext.Session.Remove("GioHang");
            return RedirectToAction("ViewProduct", "Home", "");
        }

    }
}
