using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class Invoice : Coproduct4<SalesInvoice, SimplifiedInvoice, RetailSalesReceipt, CreditInvoice>
    {
        public Invoice(SalesInvoice firstValue)
            : base(firstValue)
        {
        }

        public Invoice(SimplifiedInvoice firstValue)
            : base(firstValue)
        {
        }

        public Invoice(RetailSalesReceipt firstValue)
            : base(firstValue)
        {
        }

        public Invoice(CreditInvoice firstValue)
            : base(firstValue)
        {
        }

        public InvoiceHeader Header
        {
            get
            {
                return Match(
                    salesInvoice => salesInvoice.Header,
                    simplifiedInvoice => simplifiedInvoice.Header,
                    retailSalesReceipt => retailSalesReceipt.Header,
                    creditInvoice => creditInvoice.Header
                );
            }
        }

        public LocalInvoiceParty Issuer
        {
            get
            {
                return Match(
                    salesInvoice => salesInvoice.Issuer,
                    simplifiedInvoice => simplifiedInvoice.Issuer,
                    retailSalesReceipt => retailSalesReceipt.Issuer,
                    creditInvoice => creditInvoice.Issuer
                );
            }
        }

        public InvoiceParty Counterpart
        {
            get
            {
                return Match(
                    salesInvoice => salesInvoice.Counterpart,
                    simplifiedInvoice => null,
                    retailSalesReceipt => null,
                    creditInvoice => creditInvoice.Counterpart
                );
            }
        }

        public INonEmptyEnumerable<Payment> Payments
        {
            get
            {
                return Match(
                    salesInvoice => salesInvoice.Payments,
                    simplifiedInvoice => simplifiedInvoice.Payments,
                    retailSalesReceipt => retailSalesReceipt.Payments,
                    creditInvoice => creditInvoice.Payments
                );
            }
        }

        public long? CorrelatedInvoice
        {
            get
            {
                return Match(
                    salesInvoice => null,
                    simplifiedInvoice => null,
                    retailSalesReceipt => null,
                    creditInvoice => CorrelatedInvoice
                );
            }
        }

        public ISequenceStartingWithOne<Revenue> RevenueItems
        {
            get
            {
                return Match(
                    salesInvoice => salesInvoice.RevenueItems,
                    simplifiedInvoice => simplifiedInvoice.RevenueItems,
                    retailSalesReceipt => retailSalesReceipt.RevenueItems,
                    creditInvoice => creditInvoice.RevenueItems
                );
            }
        }
    }
}
