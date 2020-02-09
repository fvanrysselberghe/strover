namespace Payconiq.Datamodel
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public PaymentStatus Status { get; set; }

        public string CreatedAt { get; set; }

        public string ExpiresAt { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public uint Amount { get; set; }

        public string Currency { get; set; }

        public Creditor Creditor { get; set; }

    }
}