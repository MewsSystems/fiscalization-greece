using Mews.Fiscalization.Greece.Dto.Xsd;
using System;
using System.Collections.Generic;
using System.Linq;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class CurrencyCode : LimitedString
    {
        private static readonly IEnumerable<string> AllowedValues = Enum.GetValues(typeof(Currency)).OfType<Currency>().Select(v => v.ToString());
        private static readonly StringLimitation Limitation = new StringLimitation(minLength: 3, maxLength: 3, allowedValues: AllowedValues);

        public CurrencyCode(string value)
            : base(value, Limitation.ToEnumerable())
        {
        }

        protected CurrencyCode(string value, IEnumerable<StringLimitation> limitations)
            : base(value, Limitation.Concat(limitations))
        {
        }

        public static bool IsValid(string value)
        {
            return IsValid(value, Limitation.ToEnumerable());
        }

        public new static bool IsValid(string value, IEnumerable<StringLimitation> limitations)
        {
            return LimitedString.IsValid(value, Limitation.Concat(limitations));
        }
    }
}

