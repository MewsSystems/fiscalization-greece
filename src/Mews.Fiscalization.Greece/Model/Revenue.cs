using FuncSharp;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class Revenue : Coproduct2<NonNegativeRevenue, NegativeRevenue>
    {
        public Revenue(NonNegativeRevenue nonNegativeRevenue)
            : base(nonNegativeRevenue)
        {
        }

        public Revenue(NegativeRevenue negativeRevenue)
            : base(negativeRevenue)
        {
        }

        public decimal NetValue
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.NetValue.Value,
                    negativeRevenue => negativeRevenue.NetValue.Value
                );
            }
        }

        public decimal VatValue
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.VatValue.Value,
                    negativeRevenue => negativeRevenue.VatValue.Value
                );
            }
        }

        public TaxType TaxType
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.TaxType,
                    negativeRevenue => negativeRevenue.TaxType
                );
            }
        }

        public VatExemptionType? VatExemption
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.VatExemption,
                    negativeRevenue => negativeRevenue.VatExemption
                );
            }
        }

        public RevenueType RevenueType
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.RevenueType,
                    negativeRevenue => negativeRevenue.RevenueType
                );
            }
        }
    }
}
