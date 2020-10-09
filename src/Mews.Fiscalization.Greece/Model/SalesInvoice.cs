using Mews.Fiscalization.Greece.Model.Types;
using System;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public class SalesInvoice : Invoice
    {
        public SalesInvoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            IEnumerable<NonNegativeRevenue> revenueItems,
            Counterpart counterpart,
            IEnumerable<NonNegativePayment> payments = null,
            InvoiceRegistrationNumber invoiceRegistrationNumber = null,
            InvoiceRegistrationNumber cancelledByInvoiceRegistrationNumber = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber, counterpart)
        {
            if (counterpart == null)
            {
                throw new ArgumentNullException(nameof(counterpart));
            }
        }
    }
}
