﻿using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public abstract class Amount : LimitedDecimal
    {
        private static readonly DecimalLimitation Limitation = new DecimalLimitation(maxDecimalPlaces: 2);

        public Amount(decimal value, DecimalLimitation limitation)
            : base(value, Limitation.Concat(limitation.ToEnumerable()))
        {
        }

        protected new static bool IsValid(decimal value, DecimalLimitation limitation)
        {
            return IsValid(value, limitation.ToEnumerable());
        }

        protected new static bool IsValid(decimal value, IEnumerable<DecimalLimitation> limitations)
        {
            return LimitedDecimal.IsValid(value, Limitation.Concat(limitations));
        }
    }
}
