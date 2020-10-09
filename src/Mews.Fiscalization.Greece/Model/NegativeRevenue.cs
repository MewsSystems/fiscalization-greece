using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class NegativeRevenue : Revenue
    {
        public NegativeRevenue(
            NegativeAmount netValue,
            NegativeAmount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            PositiveInt lineNumber = null,
            VatExemptionType? vatExemption = null)
            : base(netValue, vatValue, taxType, revenueType, lineNumber, vatExemption)
        {
        }
    }
}
