using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class ExchangeRate : LimitedDecimal
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(min: 0, maxDecimalPlaces: 5);

        public ExchangeRate(decimal value)
            : base(value, Limitation.ToEnumerable())
        {
        }

        protected ExchangeRate(decimal value, IEnumerable<DecimalLimitation> limitations)
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
