using Mews.Fiscalization.Greece.Dto.Xsd;
using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class CurrencyCode : LimitedString
    {
        private static readonly StringLimitation Limitation = new StringLimitation(minLength: 3, maxLength: 3);

        public CurrencyCode(string value)
            : base(value, Limitation)
        {
            if (!Enum.TryParse<Currency>(value, out _))
            {
                throw new ArgumentException("Currency code is not valid ISO-4217 code.", nameof(value));
            }
        }

        public static bool IsValid(string value)
        {
            return LimitedString.IsValid(value, Limitation) && Enum.TryParse<Currency>(value, out _);
        }
    }
}

