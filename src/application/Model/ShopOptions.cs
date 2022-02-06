namespace Strover.Models
{
    public class ShopOptions
    {
        public string LegalName { get; set; }
        public string AccountNumber { get; set; }
        public string Email { get; set; }

        public bool Closed { get; set; }

        public TimePeriod DeliveryPeriod { get; set; }
        public TimePeriod PickupPeriod { get; set; }
    }
}