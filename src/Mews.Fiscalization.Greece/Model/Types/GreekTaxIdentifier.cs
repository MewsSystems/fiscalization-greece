using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class GreekTaxIdentifier : NonEmptyString
    {
        private static readonly StringLimitation Limitation = new StringLimitation(pattern: "[0-9]{9}$");

        public GreekTaxIdentifier(string value)
            : base(value, Limitation.ToEnumerable())
        {
        }

        public new static bool IsValid(string value)
        {
            return IsValid(value, Limitation.ToEnumerable());
        }

        protected new static bool IsValid(string value, IEnumerable<StringLimitation> limitations)
        {
            return NonEmptyString.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
