using System;
using System.Collections.Generic;

#nullable disable

namespace MixueShop.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int CateId { get; set; }
        
        public double? Discount { get; set; }

        public virtual Category Cate { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
