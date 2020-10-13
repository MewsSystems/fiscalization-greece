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
            : base(value, Limitation)
        {
        }

        public static bool IsValid(string value)
        {
            return IsValid(value, Limitation);
        }
    }
}

