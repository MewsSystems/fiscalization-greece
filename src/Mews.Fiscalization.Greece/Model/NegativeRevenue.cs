using Mews.Fiscalization.Core.Model;
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
            VatExemptionType? vatExemption = null)
            : base(netValue, vatValue, taxType, revenueType, vatExemption)
        {
        }
    }
}
