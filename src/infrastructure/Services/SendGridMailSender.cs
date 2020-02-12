using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Strover.Models;
using System.Threading.Tasks;

namespace Strover.Infrastructure.Services
{
    public class SendGridMailSender : IEmailSender
    {
        public SendGridMailSender(IConfiguration config, IOptions<ShopOptions> shopConfiguration)
        {
            ApiKey = config.GetValue<string>("SENDGRID_APIKEY");

            var shop = shopConfiguration.Value;
            Sender = new EmailAddress(shop.Email, shop.LegalName);
        }

        private string ApiKey { get; }
        private EmailAddress Sender { get; }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(ApiKey);
            var msg = new SendGridMessage()
            {
                From = Sender,
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg);
        }
    }
}