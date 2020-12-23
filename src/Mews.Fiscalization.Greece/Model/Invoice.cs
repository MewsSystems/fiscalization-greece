using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class Invoice : Coproduct4<SalesInvoice, SimplifiedInvoice, RetailSalesReceipt, CreditInvoice>
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

        public InvoiceParty Issuer
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
                    salesInvoice => salesInvoice.Payments.Select(p => new Payment(p)),
                    simplifiedInvoice => simplifiedInvoice.Payments.Select(p => new Payment(p)),
                    retailSalesReceipt => retailSalesReceipt.Payments.Select(p => new Payment(p)),
                    creditInvoice => creditInvoice.Payments.Select(p => new Payment(p))
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
                    creditInvoice => creditInvoice.CorrelatedInvoice
                );
            }
        }

        public ISequenceStartingWithOne<Revenue> RevenueItems
        {
            get
            {
                return Match(
                    salesInvoice => SequenceStartingWithOne.FromPreordered(salesInvoice.RevenueItems.Values.Select(r => new Revenue(r.Value))),
                    simplifiedInvoice => SequenceStartingWithOne.FromPreordered(simplifiedInvoice.RevenueItems.Values.Select(r => new Revenue(r.Value))),
                    retailSalesReceipt => SequenceStartingWithOne.FromPreordered(retailSalesReceipt.RevenueItems.Values.Select(r => new Revenue(r.Value))),
                    creditInvoice => SequenceStartingWithOne.FromPreordered(creditInvoice.RevenueItems.Values.Select(r => new Revenue(r.Value)))
                );
            }
        }
    }
}
