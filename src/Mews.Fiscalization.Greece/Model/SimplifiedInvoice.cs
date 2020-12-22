using System.Collections.Generic;
using System.Linq;
using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class SimplifiedInvoice
    {
        private SimplifiedInvoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
        {
            Header = header;
            Issuer = issuer;
            RevenueItems = revenueItems;
            Payments = payments;
            if (Header.CurrencyCode.IsNull() || Header.CurrencyCode.Value == "EUR")
            {
                Check.Condition(RevenueItems.Values.Sum(i => i.Value.NetValue.Value + i.Value.VatValue.Value) <= 100, $"{nameof(SimplifiedInvoice)} can only be below 100 EUR.");
            }
        }

        public InvoiceHeader Header { get; }

        public LocalInvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<Payment> Payments { get; }

        public static ITry<SimplifiedInvoice, IEnumerable<Error>> Create(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
        {
            return Try.Aggregate(
                NotNull(header),
                NotNull(issuer),
                NotNull(revenueItems),
                NotNull(payments),
                (h, i, r, p) => new SimplifiedInvoice(h, i, r, p)
            );
        }

        public static ITry<T, IEnumerable<Error>> NotNull<T>(T value)
        {
            return value.ToTry(v => v.IsNotNull(), _ => new Error("Value cannot be null.").ToEnumerable());
        }
    }
}
