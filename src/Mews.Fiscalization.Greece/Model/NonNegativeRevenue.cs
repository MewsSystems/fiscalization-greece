using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class NonNegativeRevenue : Revenue
    {
        public NonNegativeRevenue(
            NonNegativeAmount netValue,
            NonNegativeAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
            : base(netValue, vatValue, taxType, revenueType, vatExemption)
        {
        }
    }
}
