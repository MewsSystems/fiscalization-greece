using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class RetailSalesReceipt
    {
        private RetailSalesReceipt(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
        {
            Header = header;
            Issuer = issuer;
            RevenueItems = revenueItems;
            Payments = payments;
        }

        public InvoiceHeader Header { get; }

        public InvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NonNegativePayment> Payments { get; }

        public static ITry<RetailSalesReceipt, IEnumerable<Error>> Create(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
        {
            return Try.Aggregate(
                ObjectExtensions.NotNull(header),
                ObjectExtensions.NotNull(issuer),
                ObjectExtensions.NotNull(revenueItems),
                ObjectExtensions.NotNull(payments),
                (h, i, r, p) => new RetailSalesReceipt(h, i, r, p)
            );
        }
    }
}
