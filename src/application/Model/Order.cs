using vlaaienslag.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace vlaaienslag.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public bool Delivered { get; set; }

        public virtual ICollection<OrderedItem> OrderedItems { get; set; }
        public virtual Customer Buyer { get; set; }

        public UInt32 OrderedQuantity { get 
            { 
                //Although a Linq.Sum is nicer it is not possible with unsigned types
                UInt32 total = 0U;
                //foreach (var item in OrderedItems)
                //{
                //    total += item.Quantity; //watch out for overflow
                //}
                return total;
            } }
    }
}