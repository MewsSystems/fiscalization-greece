using Mews.Fiscalization.Core.Model;
using System;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Revenue
    {
        public Revenue(
            Amount netValue,
            Amount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
        {
            NetValue = netValue ?? throw new ArgumentNullException(nameof(netValue));
            VatValue = vatValue ?? throw new ArgumentNullException(nameof(vatValue));
            TaxType = taxType;
            RevenueType = revenueType;
            VatExemption = vatExemption;

            if (taxType == TaxType.Vat0 && !vatExemption.HasValue)
            {
                throw new ArgumentException($"{nameof(VatExemption)} must be specified when TaxType is {taxType}");
            }
        }

        public Amount NetValue { get; }

        public Amount VatValue { get; }

        public TaxType TaxType { get; }

        public RevenueType RevenueType { get; }

        public VatExemptionType? VatExemption { get; }
    }
}
