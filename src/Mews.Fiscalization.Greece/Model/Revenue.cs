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

        public RevenueInfo Info
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.Info,
                    negativeRevenue => negativeRevenue.Info
                );
            }
        }

        public TaxType TaxType
        {
            get { return Info.TaxType; }
        }

        public IOption<VatExemptionType> VatExemption
        {
            get { return Info.VatExemption; }
        }

        public RevenueType RevenueType
        {
            get { return Info.RevenueType; }
        }
    }
}
