using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class RetailSalesReceipt : Invoice
    {
        public RetailSalesReceipt(
            InvoiceHeader header,
            LocalCounterpart issuer,
            ISequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            IEnumerable<NonNegativePayment> payments = null,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber)
        {
        }
    }
}
