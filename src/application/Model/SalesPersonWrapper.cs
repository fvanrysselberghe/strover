using Strover.Models;

namespace Strover.Models
{
    public class SalesPersonWrapper
    {
        public string ID { get; set; }
        public string Name { get; set; }

        //Class in the current one, but more general it is the sales division or something
        public string Division { get; set; }
    }
}