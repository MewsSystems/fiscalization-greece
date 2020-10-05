﻿using System;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class TaxIdentifier : NotEmptyString
    {
        public TaxIdentifier(string value)
            : base(value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (!Patterns.TaxIdentifier.IsMatch(value))
            {
                throw new ArgumentException($"The value '{value}' does not match the pattern '{Patterns.TaxIdentifier}'");
            }
        }

        public new static bool IsValid(string value)
        {
            return value != null && Patterns.TaxIdentifier.IsMatch(value);
        }
    }
}
