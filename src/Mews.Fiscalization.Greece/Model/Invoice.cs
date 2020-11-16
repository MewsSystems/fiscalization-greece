using Mews.Fiscalization.Core.Model;
using System;
using System.Linq;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Invoice
    {
        public Invoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequentialEnumerableStartingWithOne<Revenue> revenueItems,
            INonEmptyEnumerable<Payment> payments,
            InvoiceParty counterpart = null,
            long? correlatedInvoice = null)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            RevenueItems = revenueItems  ?? throw new ArgumentNullException(nameof(revenueItems));
            Payments = payments ?? throw new ArgumentNullException(nameof(payments));
            Counterpart = counterpart;
            CorrelatedInvoice = correlatedInvoice;

            if (!RevenueItems.Any())
            {
                throw new ArgumentException($"Minimal count of {nameof(revenueItems)} is 1.");
            }
        }

        public InvoiceHeader Header { get; }

        public LocalInvoiceParty Issuer { get; }

        public ISequentialEnumerableStartingWithOne<Revenue> RevenueItems { get; }

        public INonEmptyEnumerable<Payment> Payments { get; }

        public InvoiceParty Counterpart { get; }

        public long? CorrelatedInvoice { get; }
    }
}
