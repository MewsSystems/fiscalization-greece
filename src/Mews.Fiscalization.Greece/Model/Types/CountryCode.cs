using System;
using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class CountryCode : LimitedString
    {
        private static readonly StringLimitation Limitation = new StringLimitation(minLength: 2, maxLength: 2);

        public CountryCode(string value)
            : this(value, Limitation.ToEnumerable())
        {
            if (!Enum.TryParse<Dto.Xsd.Country>(value, out _))
            {
                throw new ArgumentException("Country code is not valid ISO 3166 code.", nameof(value));
            }
        }

        protected CountryCode(string value, IEnumerable<StringLimitation> limitations)
            : base(value, Limitation.Concat(limitations))
        {
        }

        public static bool IsValid(string value)
        {
            return IsValid(value, Limitation.ToEnumerable()) && Enum.TryParse<Dto.Xsd.Country>(value, out _);
        }

        protected new static bool IsValid(string value, IEnumerable<StringLimitation> limitations)
        {
            return LimitedString.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
