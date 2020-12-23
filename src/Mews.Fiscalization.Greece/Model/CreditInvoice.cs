using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class CreditInvoice
    {
        private CreditInvoice(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NegativeRevenue> revenueItems,
            INonEmptyEnumerable<NegativePayment> payments,
            InvoiceParty counterpart,
            long? correlatedInvoice = null)
        {
            Header = header;
            Issuer = issuer;
            RevenueItems = revenueItems;
            Payments = payments;
            Counterpart = counterpart;
            CorrelatedInvoice = correlatedInvoice;
        }

        public InvoiceHeader Header { get; }

        public InvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NegativePayment> Payments { get; }

        public InvoiceParty Counterpart { get; }

        public long? CorrelatedInvoice { get; }

        public static ITry<CreditInvoice, IEnumerable<Error>> Create(
            InvoiceHeader header,
            InvoiceParty issuer,
            ISequenceStartingWithOne<NegativeRevenue> revenueItems,
            INonEmptyEnumerable<NegativePayment> payments,
            InvoiceParty counterPart,
            long? correlatedInvoice = null)
        {
            return Try.Aggregate(
                ObjectExtensions.NotNull(header),
                ObjectExtensions.NotNull(issuer),
                ObjectExtensions.NotNull(revenueItems),
                ObjectExtensions.NotNull(payments),
                ObjectExtensions.NotNull(counterPart),
                (h, i, r, p, c) => new CreditInvoice(h, i, r, p, c, correlatedInvoice)
            );
        }
    }
}
