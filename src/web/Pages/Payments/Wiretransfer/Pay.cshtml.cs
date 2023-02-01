using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Strover.Application;
using Strover.Infrastructure.Data;
using Strover.Models;

namespace Strover.Pages.Payments.Wiretransfer
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

        private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _mailClient;

        private readonly UserManager<SalesPerson> _users;

        public PayModel(DataStoreContext paymentStore, IOptions<ShopOptions> shopConfiguration, UserManager<SalesPerson> users, IEmailSender mailClient)
        {
            _store = paymentStore;
            _shopConfig = shopConfiguration.Value;
            _users = users;
            _mailClient = mailClient;
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

            InformAboutWiretransferLimitations(payment);

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

        /// <summary>
        /// Webshop users expect immediate feedback on their payments. Unfortunately the cheap wiretransfers don't support this
        /// therefore we allow users to send a confirmation to the one paying that confirms the payment attempt. 
        /// It also explains the  manual validation process. 
        /// </summary>
        public async void InformAboutWiretransferLimitations(Payment payment)
        {
            var user = await _users.FindByNameAsync(userName: User.Identity.Name);
            if (user == null)
                return;

            _mailClient.SendEmailAsync(
                user.Email,
                "Bedankt voor je bestelling",
                "Dummy tekst");
        }

    }
}