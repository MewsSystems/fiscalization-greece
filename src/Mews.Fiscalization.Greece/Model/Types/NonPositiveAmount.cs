using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class NonPositiveAmount : Amount
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(max: 0);

        public NonPositiveAmount(decimal value)
            : base(value, Limitation.ToEnumerable())
        {
        }

        public new static bool IsValid(decimal value)
        {
            return IsValid(value, Limitation.ToEnumerable());
        }

        protected new static bool IsValid(decimal value, IEnumerable<DecimalLimitation> limitations)
        {
            return Amount.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
