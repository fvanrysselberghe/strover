namespace vlaaienslag.Models
{
    public enum DeliveryType { Pickup, Delivery };

    // #TODO think about modelling different types of deliveries
    public class DeliveryMethod
    {
        public DeliveryType DeliveryType { get; set; }
        public Address DeliveryAddress { get; set; }
        public string Comments { get; set; }
    }
}