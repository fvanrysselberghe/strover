namespace Strover.Models
{
    public class ShopOptions
    {
        public string LegalName { get; set; }
        public string AccountNumber { get; set; }
        public string Email { get; set; }

        public bool Closed { get; set; }

        public TimePeriod DeliveryPeriod { get; set; }

        public string DeliveryLocations { get; set; }
        public TimePeriod PickupPeriod { get; set; }
        public string PickupLocations { get; set; }

        public string WireTransferText { get; set; }

    }
}