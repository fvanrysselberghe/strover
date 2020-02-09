using System;

namespace Payconiq.Datamodel
{
    public class PaymentRequest
    {
        /// <summary>
        /// Payment amount in euro cents
        /// </summary>
        /// <value></value>
        public ulong Amount { get; set; }

        /// <summary>
        /// Payment currency code in ISO 4217 format
        /// Only Euros supported at the moment
        /// </summary>
        public string Currency => _currency;
        private const string _currency = "EUR";

        /// <summary>
        /// A url which the Merchant or Partner will be notified
        /// of a payment. Must be Https for production.
        /// </summary>
        /// <value></value>
        public Uri CallbackUrl { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public string BulkId { get; set; }
    }
}