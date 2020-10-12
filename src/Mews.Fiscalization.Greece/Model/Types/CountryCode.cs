using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class CountryCode : LimitedString
    {
        private static readonly StringLimitation Limitation = new StringLimitation(minLength: 2, maxLength: 2);

        public CountryCode(string value)
            : base(value, Limitation)
        {
            if (!Enum.TryParse<Dto.Xsd.Country>(value, out _))
            {
                throw new ArgumentException("Country code is not valid ISO 3166 code.", nameof(value));
            }
        }

        public static bool IsValid(string value)
        {
            return LimitedString.IsValid(value, Limitation) && Enum.TryParse<Dto.Xsd.Country>(value, out _);
        }
    }
}
