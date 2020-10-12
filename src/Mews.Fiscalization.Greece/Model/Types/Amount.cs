using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public abstract class Amount : LimitedDecimal
    {
        protected static readonly int maxDecimalPlaces = 2;

        public Amount(decimal value, decimal? minValue = null, decimal? maxValue = null, bool includeMax = true)
            : base(value, GetLimitation(minValue: minValue, maxValue: maxValue, includeMax: includeMax))
        {
        }

        public static bool IsValid(decimal value, decimal? minValue = null, decimal? maxValue = null, bool includeMax = true)
        {
            return LimitedDecimal.IsValid(value, GetLimitation(minValue: minValue, maxValue: maxValue, includeMax: includeMax));
        }

        private static DecimalLimitation GetLimitation(decimal? minValue = null, decimal? maxValue = null, bool includeMax = true)
        {
            return new DecimalLimitation(maxDecimalPlaces: maxDecimalPlaces, min: minValue, max: maxValue, includeMax: includeMax);
        }
    }
}
