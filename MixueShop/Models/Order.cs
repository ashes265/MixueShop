using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MixueShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate {
            get;
            set; 
        }
        //public double Amount { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
