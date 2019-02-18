using System.Collections.Generic;

namespace Strover.Models
{
    public enum PaymentState
    {
        BeingProcessed,
        Paid,
        Cancelled
    }
    public class Payment
    {
        public string ID { get; set; }
        public PaymentState State { get; set; }

        public decimal Amount { get; set; }

        public virtual ICollection<OrderPayments> OrderPayments { get; set; }

    }
}