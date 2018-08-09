using vlaaienslag.Models;
using System;

namespace vlaaienslag.Models
{
    public class OrderedItem
    {
        public string OrderedItemId { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        public UInt32 Quantity { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}