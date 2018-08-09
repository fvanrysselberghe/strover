using vlaaienslag.Models;

namespace vlaaienslag.Models
{
    public class SalesPerson
    {
        public string ID { get; set; }
        public string Name { get; set; }

        //Class in the current one, but more general it is the sales division or something
        public string Division { get; set; }
    }
}