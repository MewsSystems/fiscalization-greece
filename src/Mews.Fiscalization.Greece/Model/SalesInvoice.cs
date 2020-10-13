using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Core.Model.Collections;
using System;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public class SalesInvoice : Invoice
    {
        public SalesInvoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            SequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            Counterpart counterpart,
            IEnumerable<NonNegativePayment> payments = null,
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
