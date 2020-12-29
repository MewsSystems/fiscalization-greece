using System.Collections.Generic;
using System.Linq;
using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class SimplifiedInvoice
    {
        private SimplifiedInvoice(InvoiceInfo info, ISequenceStartingWithOne<NonNegativeRevenue> revenueItems, INonEmptyEnumerable<NonNegativePayment> payments)
        {
            Info = info;
            RevenueItems = revenueItems;
            Payments = payments;
            if (Info.Header.CurrencyCode.IsNull() || info.Header.CurrencyCode.Value == "EUR")
            {
                Check.Condition(RevenueItems.Values.Sum(i => i.Value.NetValue.Value + i.Value.VatValue.Value) <= 100, $"{nameof(SimplifiedInvoice)} can only be below 100 EUR.");
            }
        }

        public InvoiceInfo Info { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NonNegativePayment> Payments { get; }

        public static ITry<SimplifiedInvoice, IEnumerable<Error>> Create(InvoiceInfo info, ISequenceStartingWithOne<NonNegativeRevenue> revenueItems, INonEmptyEnumerable<NonNegativePayment> payments)
        {
            return Try.Aggregate(
                ObjectExtensions.NotNull(info),
                ObjectExtensions.NotNull(revenueItems),
                ObjectExtensions.NotNull(payments),
                (i, r, p) => new SimplifiedInvoice(i, r, p)
            );
        }
    }
}
