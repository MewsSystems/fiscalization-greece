using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class SimplifiedInvoice : Invoice
    {
        public SimplifiedInvoice(
            InvoiceHeader header,
            LocalCounterpart issuer,
            ISequentialEnumerableStartingWithOne<NonNegativeRevenue> revenueItems,
            INonEmptyEnumerable<NonNegativePayment> payments,
            long? invoiceRegistrationNumber = null,
            long? cancelledByInvoiceRegistrationNumber = null)
            : base(header, issuer, revenueItems, payments, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber)
        {
        }
    }
}
