namespace Mews.Fiscalization.Greece.Model.Types
{
    public class NegativeAmount : Amount
    {
        public NegativeAmount(decimal value)
            : base(value, maxValue: 0, includeMax: false)
        {
        }

        public static bool IsValid(decimal value)
        {
            return Amount.IsValid(value, maxValue: 0, includeMax: false);
        }
    }
}

