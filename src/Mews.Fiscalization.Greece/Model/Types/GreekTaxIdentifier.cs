using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class GreekTaxIdentifier : NonEmptyString
    {
        private static readonly StringLimitation Limitation = new StringLimitation(pattern: "[0-9]{9}$");

        public GreekTaxIdentifier(string value)
            : base(value, Limitation)
        {
        }

        public new static bool IsValid(string value)
        {
            return IsValid(value, Limitation);
        }
    }
}
