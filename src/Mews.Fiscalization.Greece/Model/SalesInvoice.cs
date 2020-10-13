using Mews.Fiscalization.Core.Model;
using System;

namespace Mews.Fiscalization.Greece.Model
{
    public class SalesInvoice : Invoice
    {
        public SalesInvoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            ISequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            Counterpart counterpart,
            INonEmptyEnumerable<NonNegativePayment> payments,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber, counterpart)
        {
            if (counterpart == null)
            {
                throw new ArgumentNullException(nameof(counterpart));
            }
        }
    }
}
