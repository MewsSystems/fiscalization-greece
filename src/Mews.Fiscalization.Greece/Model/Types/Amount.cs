using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public abstract class Amount : LimitedDecimal
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(maxDecimalPlaces: 2);

        public Amount(decimal value)
            : this(value, Limitation.ToEnumerable())
        {
        }

        protected Amount(decimal value, IEnumerable<DecimalLimitation> limitations)
            : base(value, Limitation.Concat(limitations))
        {
        }

        public static bool IsValid(decimal value)
        {
            return IsValid(value, Limitation.ToEnumerable());
        }

        protected new static bool IsValid(decimal value, IEnumerable<DecimalLimitation> limitations)
        {
            return LimitedDecimal.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
