using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class RetailSalesReceipt : Invoice
    {
        public RetailSalesReceipt(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
            : base(header, issuer, revenueItems, payments)
        {
        }
    }
}
