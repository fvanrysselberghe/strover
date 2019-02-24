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
        public decimal Amount { get; set; };

        private readonly ShopOptions _shopConfig;

        private readonly DataStoreContext _store;

        public PayModel(DataStoreContext paymentStore, IOptions<ShopOptions> shopConfiguration)
        {
            _store = paymentStore;
            _shopConfig = shopConfiguration.Value;
        }

        public IActionResult OnGet(string paymentId)
        {
            PaymentId = paymentId;
            var payment = _store.Payment.Find(paymentId);

            if (payment == null)
                return NotFound();

            Beneficiary = _shopConfig.LegalName;
            AccountNumber = _shopConfig.AccountNumber;
            Message = payment.ID;
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

        public IActionResult OnPost(string paymentId)
        {
            //We created the payment as cancelled, user confirmed payment
            //=> Switch to processing
            var payment = _store.Payment.Find(paymentId);
            if (payment == null)
                return NotFound();

            payment.State = PaymentState.BeingProcessed;
            _store.Payment.Update(payment);
            _store.SaveChanges();

            return RedirectToPage("/Orders/Index");
        }

        public IActionResult OnGetCancel(string paymentId)
        {
            //We created the payment as cancelled, user bailed out
            // => safely return
            return RedirectToPage("/Orders/Index");
        }


    }
}