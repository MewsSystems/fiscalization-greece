using FuncSharp;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class Amount : Coproduct3<NonPositiveAmount, NonNegativeAmount, NegativeAmount>
    {
        public Amount(NonPositiveAmount firstValue)
            : base(firstValue)
        {
        }

        public Amount(NonNegativeAmount secondValue)
            : base(secondValue)
        {
        }

        public Amount(NegativeAmount thirdValue)
            : base(thirdValue)
        {
        }

        public decimal Value
        {
            get
            {
                return Match(
                    nonPositiveAmount => nonPositiveAmount.Value,
                    nonNegativeAmount => nonNegativeAmount.Value,
                    negativeAmount => negativeAmount.Value
                );
            }
        }
    }
}
