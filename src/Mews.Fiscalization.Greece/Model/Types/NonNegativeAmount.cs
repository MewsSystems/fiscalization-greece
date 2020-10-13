using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class NonNegativeAmount : Amount
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(min: 0);

        public NonNegativeAmount(decimal value)
            : base(value, Limitation)
        {
        }

        public static bool IsValid(decimal value)
        {
            return Amount.IsValid(value, Limitation);
        }
    }
}
