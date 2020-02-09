namespace Payconiq.Datamodel
{
    public class Creditor
    {
        public string ProfileId { get; set; }
        public string MerchantId { get; set; }

        public string Name { get; set; }
        public string Iban { get; set; }

        public string CallbackUrl { get; set; }
    }
}