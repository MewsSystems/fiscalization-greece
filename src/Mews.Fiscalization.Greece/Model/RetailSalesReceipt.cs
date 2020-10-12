using Mews.Fiscalization.Greece.Model.Types;
using System.Collections.Generic;

namespace Mews.Fiscalization.Greece.Model
{
    public class RetailSalesReceipt : Invoice
    {
        public RetailSalesReceipt(
            InvoiceHeader header,
            LocalCounterpart issuer,
            IEnumerable<NonNegativeRevenue> revenueItems,
            IEnumerable<NonNegativePayment> payments = null,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber)
        {
        }
    }
}
