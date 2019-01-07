using System.Collections.Generic;

namespace Strover.Models
{
    public class Product
    {
        // Key
        public string ProductId { get; set; }

        public ICollection<OrderedItem> OrderedItems { get; set; }

        // Name that is used to refer to the product like 'Kersentaart'
        public string Name { get; set; }

        // Price of the product
        public decimal Price { get; set; }

    }
}