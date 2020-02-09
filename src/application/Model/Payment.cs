using System.Collections.Generic;

namespace Strover.Models
{
    public enum PaymentState
    {
        Cancelled = 0,
        BeingProcessed = 1,
        Paid = 2
    }

    public enum PaymentMethod
    {
        WireTransfer
    }

    public class Payment
    {
        public string ID { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentState State { get; set; }

        public decimal Amount { get; set; }
        public System.DateTime ExecutionDate { get; set; }

        public string Reference { get; set; }

        public virtual ICollection<OrderPayments> OrderPayments { get; set; }
    }
}