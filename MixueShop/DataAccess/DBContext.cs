using MixueShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using MixueShop.Logic;
namespace MixueShop.DataAccess
{
    public class DBContext : BaseDAL
    {
        public static DBContext instance = null;
        private static readonly object instanceLock = new object();
        public static ProductManage mng=new ProductManage();
        public IEnumerable<TopProduct> getTopPro()
        {
            IDataReader dataReader = null;
            string SQLSelect = "select productID, sum(quantity) from OrderDetail group by productID";
            var list = new List<TopProduct>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    Product p = mng.getProByID(dataReader.GetInt32(0));
                    list.Add(new TopProduct
                    {

                        ProductName=p.ProductName,
                        Quantity=dataReader.GetInt32(1),

                    });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        public IEnumerable<OrderAdmin> getAllOrdersAmin()
        {
            IDataReader dataReader = null;
            string SQLSelect = "select o.orderID, sum((od.price*od.quantity)) as amount from OrderDetail od inner join[Order] o on od.orderID = o.orderID group by o.orderID";
            var list = new List<OrderAdmin>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    Order o = getOrderByID(dataReader.GetInt32(0));
                    list.Add(new OrderAdmin
                    {
                        OrderID = o.OrderId,
                        OrderDate = o.OrderDate,
                        Amount = dataReader.GetDouble(1)
                    }); ;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        public IEnumerable<Order> getAllOrders()
        {
            IDataReader dataReader = null;
            string SQLSelect = "select o.orderID, sum((od.price*od.quantity)) as amount from OrderDetail od inner join[Order] o on od.orderID = o.orderID group by o.orderID";
            var list = new List<Order>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    Order o = getOrderByID(dataReader.GetInt32(0));
                    list.Add(new Order
                    {
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        //Amount = dataReader.GetDouble(1)
                    }); ;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        //private DBContext() { }
        public static DBContext Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DBContext();
                    }
                    return instance;
                }
            }
        }

        public Order getOrderByID(int id)
        {
            IDataReader dataReader = null;
            string SQLSelect = "select * from [Order] where orderID=@i";
            Order order = null;
            try
            {
                var param = dataProvider.CreateParameter("@i", 4, id, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    order =new Order
                    {
                        OrderId=dataReader.GetInt32(0),
                        OrderDate=dataReader.GetDateTime(1),
                    };
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return order;
        }
        
    }
}
