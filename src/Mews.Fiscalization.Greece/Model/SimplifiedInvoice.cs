using System.Linq;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class SimplifiedInvoice : Invoice
    {
        public SimplifiedInvoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
            : base(header, issuer, revenueItems, payments)
        {
            if (header.CurrencyCode.IsNull() || header.CurrencyCode.Value == "EUR")
            {
                Check.Condition(revenueItems.Sum(i => i.Value.NetValue.Value + i.Value.VatValue.Value) <= 100, "Simplified Invoice can only be below 100 EUR.");
            }
        }
    }
}
