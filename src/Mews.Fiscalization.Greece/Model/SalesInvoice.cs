using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class SalesInvoice
    {
        private SalesInvoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
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

        public LocalInvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<NonNegativePayment> Payments { get; }

        public InvoiceParty Counterpart { get; }

        public static ITry<SalesInvoice, IEnumerable<Error>> Create(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments,
            InvoiceParty counterPart)
        {
            return Try.Aggregate(
                NotNull(header),
                NotNull(issuer),
                NotNull(revenueItems),
                NotNull(payments),
                NotNull(counterPart),
                (h, i, r, p, c) => new SalesInvoice(h, i, r, p, c)
            );
        }

        public static ITry<T, IEnumerable<Error>> NotNull<T>(T value)
        {
            return value.ToTry(v => v.IsNotNull(), _ => new Error("Value cannot be null.").ToEnumerable());
        }
    }
}
