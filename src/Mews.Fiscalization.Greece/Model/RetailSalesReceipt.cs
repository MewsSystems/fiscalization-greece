using FuncSharp;
using Mews.Fiscalization.Core.Model;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class RetailSalesReceipt
    {
        private RetailSalesReceipt(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequenceStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments)
        {
            Header = header;
            Issuer = issuer;
            RevenueItems = revenueItems;
            Payments = payments;
        }

        public InvoiceHeader Header { get; }

        public LocalInvoiceParty Issuer { get; }

        public ISequenceStartingWithOne<NonNegativeRevenue> RevenueItems { get; }

        public INonEmptyEnumerable<Payment> Payments { get; }

        public static ITry<RetailSalesReceipt, IEnumerable<Error>> Create(
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
                (h, i, r, p) => new RetailSalesReceipt(h, i, r, p)
            );
        }

        public static ITry<T, IEnumerable<Error>> NotNull<T>(T value)
        {
            return value.ToTry(v => v.IsNotNull(), _ => new Error("Value cannot be null.").ToEnumerable());
        }
    }
}
