namespace Strover.Models
{
    /// <summary>
    /// Structured reference is a formatted reference for payments.
    /// It follows the Belian OCS-VCS standard. 
    /// 
    /// The representation of the reference depedends on the context.
    /// 
    /// </summary>
    public class StructuredReference
    {
        public StructuredReference(ulong value)
        {
            _value = Truncate(value);
            _crc = (short)(_value % 97);
        }

        private const ulong MAX_VALUE = 9_999_999_999ul;

        private readonly ulong _value = 0ul;
        private readonly short _crc = 0;

        private ulong Truncate(ulong value)
        {
            return value;
        }

        /// <summary>
        /// Representation of the reference that can be used in printed
        /// documents or visual representations
        /// 
        /// According to the OGS-VCS standard the format is defined as:
        /// "+++ [0-9]3/[0-9]4/[0-9]5 +++" 
        /// </summary>
        /// <returns>Representation for printed materials</returns>
        public string AsPrintReference()
        {
            var raw = AsElectoricReference();

            return System.String.Format("+++{0}/{1}/{2}+++",
                                        raw.Substring(0, 3),
                                        raw.Substring(3, 4),
                                        raw.Substring(7, 5));
        }

        /// <summary>
        /// Representation of the reference that is used in electronic
        /// media (e.g. QR-codes)
        /// 
        /// </summary>
        /// <returns>Represenation for electronic media</returns>
        public string AsElectoricReference()
        {
            return System.String.Format("{0:D10}{1:D2}", _value, _crc);
        }
    }
}