using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class CreditInvoice : Invoice
    {
        public CreditInvoice(
            InvoiceHeader header,
            LocalInvoiceParty issuer,
            ISequentialEnumerableStartingWithOne<NegativeRevenue> revenueItems,
            InvoiceParty counterpart,
            INonEmptyEnumerable<NegativePayment> payments,
            long? correlatedInvoice = null)
            : base(header, issuer, revenueItems, payments, counterpart, correlatedInvoice)
        {
            if (counterpart == null)
            {
                throw new ArgumentNullException(nameof(counterpart));
            }
        }
    }
}
