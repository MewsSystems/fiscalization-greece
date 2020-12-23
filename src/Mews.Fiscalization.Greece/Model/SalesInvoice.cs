using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class SalesInvoice
    {
        private SalesInvoice(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments,
            InvoiceParty counterpart)
        {
            Header = header;
            Issuer = issuer;
            RevenueItems = revenueItems;
            Payments = payments;
            Counterpart = counterpart;
        }

        public InvoiceHeader Header { get; }

        public InvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NonNegativePayment> Payments { get; }

        public InvoiceParty Counterpart { get; }

        public static ITry<SalesInvoice, IEnumerable<Error>> Create(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments,
            InvoiceParty counterPart)
        {
            return Try.Aggregate(
                ObjectExtensions.NotNull(header),
                ObjectExtensions.NotNull(issuer),
                ObjectExtensions.NotNull(revenueItems),
                ObjectExtensions.NotNull(payments),
                ObjectExtensions.NotNull(counterPart),
                (h, i, r, p, c) => new SalesInvoice(h, i, r, p, c)
            );
        }
    }
}
