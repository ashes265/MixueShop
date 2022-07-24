using System;
using System.Collections.Generic;

#nullable disable

namespace MixueShop.Models
{
    public partial class OrderDetail
    {
        public int DetailOrderId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        //public double OdAmount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
