using System.Collections.Generic;

namespace Strover.Models
{

    public class OrderRequest
    {
        public Customer Buyer { get; set; }
        public SalesPerson Seller { get; set; }
        public IDictionary<string, uint> Items { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}