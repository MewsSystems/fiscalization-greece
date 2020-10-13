using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Core.Model.Collections;
using Mews.Fiscalization.Greece.Model.Collections;
using Mews.Fiscalization.Greece.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Invoice
    {
        public Invoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            IEnumerable<Revenue> revenueItems,
            IEnumerable<Payment> payments = null,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null,
            Counterpart counterpart = null,
            long? correlatedInvoice = null)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            RevenueItems = SequentialEnumerableStartingWithOne.Create(revenueItems.Select((item, index) => new IndexedItem<Revenue>(index, item)));
            Counterpart = counterpart;
            Payments = payments;
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

        public ISequentialEnumerable<Revenue> RevenueItems { get; }

        public Counterpart Counterpart { get; }

        public IEnumerable<Payment> Payments { get; }

        public long? InvoiceRegistrationNumber { get; }

        public long? CanceledByInvoiceRegistrationNumber { get; }

        public long? CorrelatedInvoice { get; }
    }
}
