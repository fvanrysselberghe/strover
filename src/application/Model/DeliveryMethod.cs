namespace vlaaienslag.Models
{
    public enum DeliveryType { Pickup, Delivery };

    public class DeliveryMethod
    {
        public DeliveryType DeliveryType { get; set; }
        public Address DeliveryAddress { get; set; }
        public string Comments { get; set; }
    }
}