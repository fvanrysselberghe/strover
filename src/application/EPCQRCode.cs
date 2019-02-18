
using System.IO;

namespace Strover.Application
{
    public class EPCQRCode
    {
        public const string eccLevel = "M";

        private const string serviceTag = "BCD";

        private const string identificationCode = "SCT";

        private const int characterset = 1;

        private const string version = "002";

        private readonly string accountNumberBeneficiary, bicBeneficiary, nameBeneficiary, purposeOfCreditTransfer, remittanceInformation, beneficiaryToOriginatorInformation;
        private readonly decimal amount;
        private readonly TypeOfRemittance typeOfRemittance;

        public EPCQRCode(string iban
 , string nameOfBeneficiary
 , decimal amount
 , string bic = ""
 , string remittanceInformation = ""
 , TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured
 , string purposeOfCreditTransfer = ""
 , string messageToBeneficiary = "")
        {
            this.nameBeneficiary = nameOfBeneficiary;
            this.amount = amount;
            this.bicBeneficiary = bic;
            this.accountNumberBeneficiary = iban;
            this.remittanceInformation = remittanceInformation;
            this.typeOfRemittance = typeOfRemittance;
            this.purposeOfCreditTransfer = purposeOfCreditTransfer;
            this.beneficiaryToOriginatorInformation = messageToBeneficiary;
        }

        public override string ToString()
        {
            var writer = new StringWriter();
            writer.NewLine = "\\n";

            writer.WriteLine(serviceTag);
            writer.WriteLine(version);
            writer.WriteLine((int)characterset);
            writer.WriteLine(identificationCode);
            writer.WriteLine(bicBeneficiary);
            writer.WriteLine(nameBeneficiary);
            writer.WriteLine(accountNumberBeneficiary);
            writer.WriteLine($"EUR" + amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            writer.WriteLine(purposeOfCreditTransfer);
            writer.WriteLine(GetStructuredRemittanceInformation());//structured
            writer.WriteLine(GetUnstructuredRemittanceInformation());//unstructured
            writer.Write(beneficiaryToOriginatorInformation);


            return writer.ToString();
        }

        public string GetStructuredRemittanceInformation()
        {
            return typeOfRemittance == TypeOfRemittance.Structured ? remittanceInformation : string.Empty;
        }

        public string GetUnstructuredRemittanceInformation()
        {
            return typeOfRemittance == TypeOfRemittance.Unstructured ? remittanceInformation : string.Empty;
        }

        public enum TypeOfRemittance
        {
            Structured,
            Unstructured
        }

    }
}