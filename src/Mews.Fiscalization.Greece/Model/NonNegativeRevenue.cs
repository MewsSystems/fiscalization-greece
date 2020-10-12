using Mews.Fiscalization.Core.Model;
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
            PositiveInt lineNumber = null,
            VatExemptionType? vatExemption = null)
            : base(netValue, vatValue, taxType, revenueType, lineNumber, vatExemption)
        {
        }
    }
}
