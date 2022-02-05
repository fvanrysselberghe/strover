using System.Collections.Generic;

namespace Strover.Models
{
    public class Product
    {
        // Key
        public string ProductId { get; set; }

        //public ICollection<OrderedItem> OrderedItems { get; set; }

        // Name that is used to refer to the product like 'Kersentaart'
        public string Name { get; set; }

        // Price of the product
        public decimal Price { get; set; }

        public int SequenceNumber { get; set; }

        // Location of the image for this product
        public string ImageLocation { get; set; }

    }
}