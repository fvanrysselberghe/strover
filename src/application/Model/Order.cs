using Strover.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strover.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public bool Delivered { get; set; }

        public string DeliveryMethodId { get; set; }

        public virtual DeliveryMethod Delivery { get; set; }

        public virtual ICollection<OrderedItem> OrderedItems { get; set; }
        public virtual Customer Buyer { get; set; }

        public UInt32 OrderedQuantity
        {
            get
            {
                if (OrderedItems == null)
                    return 0;

                //Although a Linq.Sum is nicer it is not possible with unsigned types
                UInt32 total = 0U;
                foreach (var item in OrderedItems)
                {
                    total += item.Quantity; //watch out for overflow
                }
                return total;
            }
        }

        public decimal Cost
        {
            get
            {
                if (OrderedItems == null)
                    return 0;

                return OrderedItems.Sum(item => item.Price);
            }
        }

        public virtual ICollection<OrderPayments> Payments { get; set; }

        public PaymentState PaymentState
        {
            get
            {
                var payments = Payments;
                if (payments == null)
                    return PaymentState.Cancelled;

                if (payments.Any(orderPayment => orderPayment.Payment.State == PaymentState.Paid))
                    return PaymentState.Paid;
                else if (payments.Any(OrderPayments => OrderPayments.Payment.State == PaymentState.BeingProcessed))
                    return PaymentState.BeingProcessed;
                else
                    return PaymentState.Cancelled;
            }
        }

    }
}