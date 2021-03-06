namespace Strover.Models
{
    public enum DeliveryType { Pickup, Delivery };

    // #TODO think about modelling different types of deliveries
    public class DeliveryMethod
    {
        public string DeliveryMethodId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public Address DeliveryAddress { get; set; }
        public string Comments { get; set; }
    }
}