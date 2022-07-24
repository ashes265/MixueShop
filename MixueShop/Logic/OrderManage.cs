using MixueShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MixueShop.DataAccess;
namespace MixueShop.Logic
{
    public class OrderManage
    {
        MixueProjectContext db=new MixueProjectContext();
        DBContext dbContext = new DBContext();
        public List<Order> getAllOrders()
        {
            return db.Orders.ToList();

        }
        public List<Order> getAllOrders(int Offset, int Count, string from, string to)
        {
            if(from!=null && to != null)
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime toDate = Convert.ToDateTime(to);
                //var list = from o in db.Orders
                //           select new Order
                //           {
                //               OrderId = o.OrderId,
                //               OrderDate = o.OrderDate,
                //           };
                var list = dbContext.getAllOrders();
                //return db.Orders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).Skip(Offset - 1).Take(Count).ToList();
                return list.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).Skip(Offset - 1).Take(Count).ToList();
            }
            else
            {
                //var list = (from o in db.Orders
                //           select new Order
                //           {
                //               OrderId = o.OrderId,
                //               OrderDate = o.OrderDate,
                //               OrderDetails = getAllDetailOfOrder(o.OrderId)
                //           }).ToList();
                var list = dbContext.getAllOrders().Skip(Offset - 1).Take(Count).ToList();
                return list;
            }
            

        }

        public List<OrderAdmin> getAllOrdersAmin(int Offset, int Count, string from, string to)
        {
            if (from != null && to != null)
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime toDate = Convert.ToDateTime(to);
                var list = dbContext.getAllOrdersAmin();
                //return db.Orders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).Skip(Offset - 1).Take(Count).ToList();
                return list.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).Skip(Offset - 1).Take(Count).ToList();
            }
            else
            {
                var list = dbContext.getAllOrdersAmin().Skip(Offset - 1).Take(Count).ToList();
                return list;
            }


        }
        public List<OrderDetail> getAllDetailOfOrder(int odID)
        {
            return db.OrderDetails.Where(o => o.OrderId == odID).ToList();
        }
        public int getNumberOrder(string from, string to)
        {
            if (from != null && to != null)
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime toDate = Convert.ToDateTime(to);
                return db.Orders.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate).Count();
            }
            else
            {
                return db.Orders.Count();
            }
        }
    }
}
