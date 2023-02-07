using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Strover.Models;
using System.Threading.Tasks;

namespace Strover.Infrastructure.Services
{
    public class SendinblueMailSender : IEmailSender
    {
        public SendinblueMailSender(IConfiguration config, IOptions<ShopOptions> shopConfiguration, ILogger<SendinblueMailSender> logger)
        {
            ApiKey = config.GetValue<string>("SENDINBLUE_APIKEY");

            var shop = shopConfiguration.Value;
            Sender = new SendSmtpEmailSender(shop.LegalName, shop.Email);
            _logger = logger;
        }

        private string ApiKey { get; }
        private SendSmtpEmailSender Sender { get; }
        private readonly ILogger _logger;

        public async System.Threading.Tasks.Task SendEmailAsync(string toAddress, string subject, string message)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Configuration.Default.AddApiKey("api-key", ApiKey);

                var apiInstance = new TransactionalEmailsApi();
                var email = new SendSmtpEmail()
                {
                    Sender = this.Sender,
                    Subject = subject,
                    TextContent = message,
                    HtmlContent = message
                };
                email.To.Add(new SendSmtpEmailTo(toAddress));

                try
                {
                    // Send a transactional email
                    CreateSmtpEmail result = apiInstance.SendTransacEmail(email);
                }
                catch (ApiException sendException)
                {
                    _logger.LogError("Failed to send e-mail: " + sendException.Message);
                }
            });
        }
    }
}