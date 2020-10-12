using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class ExchangeRate : LimitedDecimal
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(min: 0, maxDecimalPlaces: 5);

        public ExchangeRate(decimal value)
            : base(value, Limitation)
        {
        }

        public static bool IsValid(decimal value)
        {
            return IsValid(value, Limitation);
        }
    }
}
