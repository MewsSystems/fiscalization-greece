using Mews.Fiscalization.Greece.Model.Types;
using System;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public class InvoiceHeader
    {
        public InvoiceHeader(
            LimitedString1To50 invoiceSeries,
            LimitedString1To50 invoiceSerialNumber,
            DateTime invoiceIssueDate,
            string invoiceIdentifier = null,
            CurrencyCode currencyCode = null,
            ExchangeRate exchangeRate = null)
        {
            InvoiceSeries = invoiceSeries ?? throw new ArgumentNullException(nameof(invoiceSeries));
            InvoiceSerialNumber = invoiceSerialNumber ?? throw new ArgumentNullException(nameof(invoiceSerialNumber));
            InvoiceIssueDate = invoiceIssueDate;
            InvoiceIdentifier = invoiceIdentifier;
            CurrencyCode = currencyCode;
            ExchangeRate = exchangeRate;
        }

        public LimitedString1To50 InvoiceSeries { get; }

        public LimitedString1To50 InvoiceSerialNumber { get; }

        public DateTime InvoiceIssueDate { get; }

        public string InvoiceIdentifier { get; }

        public CurrencyCode CurrencyCode { get; }

        public ExchangeRate ExchangeRate { get; }
    }
}
