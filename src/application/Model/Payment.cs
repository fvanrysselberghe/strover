using System.Collections.Generic;

namespace Strover.Models
{
    public enum PaymentState
    {
        BeingProcessed,
        Paid,
        Cancelled
    }

    public enum PaymentMethod
    {
        Wire
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