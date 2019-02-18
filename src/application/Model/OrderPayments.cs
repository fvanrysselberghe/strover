namespace Strover.Models
{
    public class OrderPayments
    {
        public string OrderPaymentsId;

        public string OrderId { get; set; }

        public Order Order { get; set; }
        public string PaymentId { get; set; }

        public Payment Payment { get; set; }
    }
}