using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Strover.Application;
using Strover.Models;

namespace Strover.Pages.Payments
{
    public class PayModel : PageModel
    {
        public string PayCode;
        public string PaymentId;

        [Display(Name = "Beneficiary")]
        public string Beneficiary { get; set; }

        [Display(Name = "Account number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Reference")]
        public string Message { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        private readonly ShopOptions _shopConfig;

        private readonly DataStoreContext _store;

        public PayModel(DataStoreContext paymentStore, IOptions<ShopOptions> shopConfiguration)
        {
            _store = paymentStore;
            _shopConfig = shopConfiguration.Value;
        }

        /// <summary>
        /// Show the payment page for a wire transfer. When we get here the payment-object is created as "BeingProcessed".
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public IActionResult OnGet(string paymentId)
        {
            PaymentId = paymentId;
            var payment = _store.Payment.Find(paymentId);

            if (payment == null)
                return NotFound();

            Beneficiary = _shopConfig.LegalName;
            AccountNumber = _shopConfig.AccountNumber;
            Message = payment.Reference;
            Amount = payment.Amount;

            var qrPayloadGenerator = new EPCQRCode(
                iban: AccountNumber,
                nameOfBeneficiary: Beneficiary,
                amount: Amount,
                remittanceInformation: Message
            );

            PayCode = qrPayloadGenerator.ToString();

            return Page();
        }


        /// <summary>
        /// The user confirmed that he made the payment. Unfortunately, we can't automatically verify whether the wiretransfer was done.
        /// Therefore we keep the current state. 
        /// Administrators will later check the account's balance. Based on the balance they can confirm payments.
        /// </summary>
        /// <param name="paymentId">Identification of the payment that was paid</param>
        /// <returns></returns>
        public IActionResult OnPost(string paymentId)
        {
            if (paymentId == null)
                return NotFound();

            var payment = _store.Payment.Find(paymentId);
            if (payment == null)
                return NotFound();

            //We created the payment as being-processed, user confirmed payment
            //=> no state change, until Administrator confirms

            return RedirectToPage("/Orders/Index");
        }

        public IActionResult OnGetCancel(string paymentId)
        {
            //We created the payment as being-processed, user pushed cancel button
            // => safely return
            return RedirectToPage("/Orders/Index");
        }


    }
}