using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class NonNegativeAmount : Amount
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(min: 0);

        public NonNegativeAmount(decimal value)
            : base(value, Limitation.ToEnumerable())
        {
        }

        public new static bool IsValid(decimal value)
        {
            return IsValid(value, Limitation.ToEnumerable());
        }

        public new static bool IsValid(decimal value, IEnumerable<DecimalLimitation> limitations)
        {
            return Amount.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
