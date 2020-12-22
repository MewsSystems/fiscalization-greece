using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class NonNegativeRevenue
    {
        private NonNegativeRevenue(
            NonNegativeAmount netValue,
            NonNegativeAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
        {
            NetValue = netValue;
            VatValue = vatValue;
            TaxType = taxType;
            RevenueType = revenueType;
            VatExemption = vatExemption;
        }

        public NonNegativeAmount NetValue { get; }

        public NonNegativeAmount VatValue { get; }

        public TaxType TaxType { get; }

        public RevenueType RevenueType { get; }

        public VatExemptionType? VatExemption { get; }

        public static ITry<NonNegativeRevenue, Error> Create(
            NonNegativeAmount netValue,
            NonNegativeAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
        {
            if (taxType == TaxType.Vat0 && !vatExemption.HasValue)
            {
                return Try.Error<NonNegativeRevenue, Error>(new Error($"{nameof(VatExemption)} must be specified when TaxType is {taxType}"));
            }
            return ObjectValidations.NotNull(netValue).FlatMap(n =>
            {
                var validVat = ObjectValidations.NotNull(vatValue);
                return validVat.Map(v => new NonNegativeRevenue(n, v, taxType, revenueType, vatExemption));
            });
        }
    }
}
