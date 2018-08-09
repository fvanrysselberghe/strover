using vlaaienslag.Models;
using System.Collections.Generic;

namespace vlaaienslag.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public bool Delivered { get; set; }

        public virtual ICollection<OrderedItem> OrderedItems { get; set; }
    }
}