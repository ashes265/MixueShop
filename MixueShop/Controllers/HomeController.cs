using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MixueShop.Models;
using System.Linq;
using MixueShop.Helpers;
using System.Collections.Generic;

namespace MixueShop.Controllers
{
    public class HomeController : Controller
    {
        MixueProjectContext db = new MixueProjectContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account a)
        {
            int check = 0;
            var listacc = db.Accounts.ToList();
            var account = new Account();
            foreach (var item in listacc)
            {
                if (item.Username.ToString().Equals(a.Username) && item.UPassword.ToString().Equals(a.UPassword))
                {
                    check++;
                    account = item;
                }
            }

            if (check != 0 && account!=null)
            {
                if (account.RoleId == 1)
                {
                    HttpContext.Session.SetString("name", account.Fullname);
                    HttpContext.Session.SetInt32("role", account.RoleId);
                    return RedirectToAction("ListOrders", "Admin");
                }
                HttpContext.Session.SetString("name", account.Fullname);
                HttpContext.Session.SetInt32("role", account.RoleId);
                return RedirectToAction("ViewProduct");
            }
            else
            {
                ViewBag.error = "Tài khoản hoặc mật khẩu sai";
                return RedirectToAction("Index");
            }
        }
        public IActionResult ViewProduct()
        {
            var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double Amount = 0.0;
            if (data!=null){
                foreach (var a in data)
                {
                    Amount += a.TotalMoney;
                }
            }
            
            ViewBag.Amount = Amount;
            ViewBag.carts = data;
            ViewBag.Name = HttpContext.Session.GetString("name");
            ViewBag.Role = HttpContext.Session.GetInt32("role");
            ViewBag.cate = db.Categories.ToList();
            ViewBag.products = db.Products.ToList();
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("name") != null)
            {
                HttpContext.Session.Remove("name");
                HttpContext.Session.Remove("role");
            }

            return RedirectToAction("Index");
        }

        

        [HttpGet("Home/Filter/{cateid}")]
        public IActionResult Filter(int cateid)
        {
            var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double Amount = 0.0;
            if (data != null)
            {
                foreach (var a in data)
                {
                    Amount += a.TotalMoney;
                }
            }

            ViewBag.Amount = Amount;
            ViewBag.carts = data;
            var products = db.Products.ToList();
            if (cateid != 0)
            {
                products = (from b in db.Products
                            where b.CateId == cateid
                            select b).ToList();
            }

           
            ViewBag.cate = db.Categories.ToList();
            ViewBag.products = products;
            ViewBag.selectedCate = cateid;
            ViewBag.Name = HttpContext.Session.GetString("name");
            ViewBag.Role = HttpContext.Session.GetInt32("role");
            return View("ViewProduct");
        }
        public IActionResult Search(string searchString)
        {
            var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
            double Amount = 0.0;
            if (data != null)
            {
                foreach (var a in data)
                {
                    Amount += a.TotalMoney;
                }
            }

            ViewBag.Amount = Amount;
            ViewBag.carts = data;
            var products = db.Products.ToList();
            ViewBag.keySearch = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                products = (from p in db.Products
                            where p.ProductName.Contains(searchString)
                            select p).ToList();
            }
           
            ViewBag.cate = db.Categories.ToList();
            ViewBag.products = products;
            ViewBag.Name = HttpContext.Session.GetString("name");
            ViewBag.Role = HttpContext.Session.GetInt32("role");
            return View("ViewProduct");
        }


    }
}
