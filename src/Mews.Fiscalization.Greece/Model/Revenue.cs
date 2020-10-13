using Mews.Fiscalization.Greece.Model.Types;
using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Revenue
    {
        public Revenue(
            Amount netValue,
            Amount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            PositiveInt lineNumber = null,
            VatExemptionType? vatExemption = null)
        {
            NetValue = netValue ?? throw new ArgumentNullException(nameof(netValue));
            VatValue = vatValue ?? throw new ArgumentNullException(nameof(vatValue));
            TaxType = taxType;
            RevenueType = revenueType;
            LineNumber = lineNumber ?? new PositiveInt(1);
            VatExemption = vatExemption;

            if (taxType == TaxType.Vat0 && !vatExemption.HasValue)
            {
                throw new ArgumentException($"{nameof(VatExemption)} must be specified when TaxType is {taxType}");
            }
        }

        public PositiveInt LineNumber { get; }

        public Amount NetValue { get; }

        public Amount VatValue { get; }

        public TaxType TaxType { get; }

        public RevenueType RevenueType { get; }

        public VatExemptionType? VatExemption { get; }
    }
}
