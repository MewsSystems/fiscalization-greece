using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class CreditInvoice
    {
        private CreditInvoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
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

        public LocalInvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NegativePayment> Payments { get; }

        public InvoiceParty Counterpart { get; }

        public long? CorrelatedInvoice { get; }

        public static ITry<CreditInvoice, IEnumerable<Error>> Create(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequenceStartingWithOne<NegativeRevenue> revenueItems,
            INonEmptyEnumerable<NegativePayment> payments,
            InvoiceParty counterPart,
            long? correlatedInvoice = null)
        {
            return Try.Aggregate(
                NotNull(header),
                NotNull(issuer),
                NotNull(revenueItems),
                NotNull(payments),
                NotNull(counterPart),
                (h, i, r, p, c) => new CreditInvoice(h, i, r, p, c, correlatedInvoice)
            );
        }

        public static ITry<T, IEnumerable<Error>> NotNull<T>(T value)
        {
            return value.ToTry(v => v.IsNotNull(), _ => new Error("Value cannot be null.").ToEnumerable());
        }
    }
}
