﻿using Mews.Fiscalization.Greece.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mews.Fiscalization.Greece.Model
{
    public class Invoice : InvoiceBase
    {
        public Invoice(
            LocalCompany issuer,
            InvoiceHeader header,
            IEnumerable<RevenueItem> revenueItems,
            StringIdentifier invoiceIdentifier = null,
            InvoiceRegistrationNumber invoiceRegistrationNumber = null,
            InvoiceRegistrationNumber cancelledByInvoiceRegistrationNumber = null,
            ForeignCompany counterpart = null,
            IEnumerable<Payment> payments = null)
            : base(issuer, header, revenueItems, invoiceIdentifier, invoiceRegistrationNumber, cancelledByInvoiceRegistrationNumber, counterpart, payments)
        {
        }
    }
}
