using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class NegativeRevenue : Revenue
    {
        public NegativeRevenue(
            NegativeAmount netValue,
            NonPositiveAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
            : base(netValue, vatValue, taxType, revenueType, vatExemption)
        {
        }
    }
}
