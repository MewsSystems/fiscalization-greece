using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class NegativeRevenue
    {
        private NegativeRevenue(
            NegativeAmount netValue,
            NonPositiveAmount vatValue,
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

        public NegativeAmount NetValue { get; }

        public NonPositiveAmount VatValue { get; }

        public TaxType TaxType { get; }

        public RevenueType RevenueType { get; }

        public VatExemptionType? VatExemption { get; }

        public static ITry<NegativeRevenue, Error> Create(
            NegativeAmount netValue,
            NonPositiveAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
        {
            if (taxType == TaxType.Vat0 && !vatExemption.HasValue)
            {
                return Try.Error<NegativeRevenue, Error>(new Error($"{nameof(VatExemption)} must be specified when TaxType is {taxType}"));
            }    
            return ObjectValidations.NotNull(netValue).FlatMap(n =>
            {
                var validVat = ObjectValidations.NotNull(vatValue);
                return validVat.Map(v => new NegativeRevenue(n, v, taxType, revenueType, vatExemption));
            });
        }
    }
}
