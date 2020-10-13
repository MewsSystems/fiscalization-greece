using Mews.Fiscalization.Core.Model;
using System;
using System.Linq;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Invoice
    {
        public Invoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            ISequentialEnumerableStartingWithOne<Revenue> revenueItems,
            INonEmptyEnumerable<Payment> payments,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null,
            Counterpart counterpart = null,
            long? correlatedInvoice = null)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            RevenueItems = revenueItems  ?? throw new ArgumentNullException(nameof(revenueItems));;
            Payments = payments ?? throw new ArgumentNullException(nameof(payments));
            Counterpart = counterpart;
            InvoiceRegistrationNumber = invoiceRegistrationNumber;
            CanceledByInvoiceRegistrationNumber = cancelledByInvoiceRegistrationNumber;
            CorrelatedInvoice = correlatedInvoice;

            if (!RevenueItems.Any())
            {
                throw new ArgumentException($"Minimal count of {nameof(revenueItems)} is 1.");
            }
        }

        public InvoiceHeader Header { get; }

        public LocalCounterpart Issuer { get; }

        public ISequentialEnumerableStartingWithOne<Revenue> RevenueItems { get; }

        public INonEmptyEnumerable<Payment> Payments { get; }

        public Counterpart Counterpart { get; }

        public long? InvoiceRegistrationNumber { get; }

        public long? CanceledByInvoiceRegistrationNumber { get; }

        public long? CorrelatedInvoice { get; }
    }
}
