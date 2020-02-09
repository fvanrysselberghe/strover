namespace Payconiq.Datamodel
{
    public class CallbackRequest
    {
        public string PaymentId { get; set; }
        public ulong Amount { get; set; }
        public ulong TransferAmount { get; set; }

        public ulong TipppingAmount { get; set; }

        public ulong TotalAmount { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public string CreatedAt { get; set; }

        public string ExpiresAt { get; set; }

        public string SucceededAt { get; set; }

        public PaymentStatus Status { get; set; }

        public Debtor Debtor { get; set; }


    }
}