using System;
using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class CreditInvoice : Invoice
    {
        public CreditInvoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            ISequentialEnumerableStartingWithOne<NegativeRevenue> revenueItems,
            Counterpart counterpart,
            IEnumerable<NegativePayment> payments = null,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null,
            long? correlatedInvoice = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber, counterpart, correlatedInvoice)
        {
            if (counterpart == null)
            {
                throw new ArgumentNullException(nameof(counterpart));
            }
        }
    }
}
