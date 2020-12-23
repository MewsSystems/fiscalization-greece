using Mews.Fiscalization.Greece.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Tests.IntegrationTests
{
    internal static class AadeTestInvoicesData
    {
        private static readonly string UserVatNumber = "";

        static AadeTestInvoicesData()
        {
            UserVatNumber = Environment.GetEnvironmentVariable("user_vat_number") ?? "INSERT_USER_VAT_NUMBER";
        }

        public static IEnumerable<object[]> GetInvoices()
        {
            var invoices = new List<object>
            {
                RetailSalesReceiptForCustomer(),
                SalesInvoiceForCompany(),
                InvoiceForForeignCompany(),
                InvoiceForForeignCompany(),
                SimplifiedInvoiceForCustomer(),
                CreditInvoiceNonAssociated(),
                CreditInvoiceNonAssociatedForForeignCompany(),
                CreditInvoiceNonAssociatedForForeignCompany()
            };
            return invoices.Select(i => new[] { i });
        }

        private static ISequenceStartingWithOne<Invoice> RetailSalesReceiptForCustomer()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(RetailSalesReceipt.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(53.65m).Success.Get(), NonNegativeAmount.Create(12.88m).Success.Get(), TaxType.Vat6, RevenueType.Products).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(53.65m).Success.Get(), NonNegativeAmount.Create(12.88m).Success.Get(), TaxType.Vat6, RevenueType.Services).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(53.65m).Success.Get(), NonNegativeAmount.Create(12.88m).Success.Get(), TaxType.Vat6, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NonNegativePayment.Create(NonNegativeAmount.Create(133.06m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(66.53m).Success.Get(), PaymentType.Cash).Success.Get()
                    )
                ).Success.Get())
            }).Get();
        }

        private static ISequenceStartingWithOne<Invoice> SalesInvoiceForCompany()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(SalesInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(88.50m).Success.Get(), NonNegativeAmount.Create(11.50m).Success.Get(), TaxType.Vat13, RevenueType.Products).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(88.50m).Success.Get(), NonNegativeAmount.Create(11.50m).Success.Get(), TaxType.Vat13, RevenueType.Services).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(88.50m).Success.Get(), NonNegativeAmount.Create(11.50m).Success.Get(), TaxType.Vat13, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.OnCredit).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    ),
                    counterPart: InvoiceParty.Create(country, TaxpayerIdentificationNumber.Create(country, "090701900").Success.Get()).Success.Get()
                ).Success.Get())
            }).Get();
        }

        private static ISequenceStartingWithOne<Invoice> InvoiceForForeignCompany()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(SalesInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(100m).Success.Get(), NonNegativeAmount.Create(0m).Success.Get(), TaxType.Vat0, RevenueType.Products, VatExemptionType.VatIncludedArticle43).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(100m).Success.Get(), NonNegativeAmount.Create(0m).Success.Get(), TaxType.Vat0, RevenueType.Services, VatExemptionType.VatIncludedArticle43).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(100m).Success.Get(), NonNegativeAmount.Create(0m).Success.Get(), TaxType.Vat0, RevenueType.Other, VatExemptionType.VatIncludedArticle43).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.OnCredit).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(100m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    ),
                    counterPart: InvoiceParty.Create(country, TaxpayerIdentificationNumber.Create(country, "090701900").Success.Get(), NonNegativeInt.CreateUnsafe(0), address: new Address(postalCode: NonEmptyString.CreateUnsafe("12"), city: NonEmptyString.CreateUnsafe("City"))).Success.Get()
                ).Success.Get())
            }).Get();
        }

        private static ISequenceStartingWithOne<Invoice> SimplifiedInvoiceForCustomer()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(SimplifiedInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(20.50m).Success.Get(), NonNegativeAmount.Create(10.50m).Success.Get(), TaxType.Vat13, RevenueType.Products).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(20.50m).Success.Get(), NonNegativeAmount.Create(10.50m).Success.Get(), TaxType.Vat13, RevenueType.Services).Success.Get(),
                        NonNegativeRevenue.Create(NonNegativeAmount.Create(20.50m).Success.Get(), NonNegativeAmount.Create(10.50m).Success.Get(), TaxType.Vat13, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NonNegativePayment.Create(NonNegativeAmount.Create(31m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(31m).Success.Get(), PaymentType.OnCredit).Success.Get(),
                        NonNegativePayment.Create(NonNegativeAmount.Create(31m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    )
                ).Success.Get())
            }).Get();
        }

        private static ISequenceStartingWithOne<Invoice> CreditInvoiceNonAssociated()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(CreditInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Products).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Services).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.OnCredit).Success.Get(),
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    ),
                    counterPart: InvoiceParty.Create(country, TaxpayerIdentificationNumber.Create(country, "090701900").Success.Get(), address: new Address(postalCode: NonEmptyString.CreateUnsafe("12"), city: NonEmptyString.CreateUnsafe("City"))).Success.Get()
                ).Success.Get())
            }).Get();
        }

        private static ISequenceStartingWithOne<Invoice> CreditInvoiceNonAssociatedForForeignCompany()
        {
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            return SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(CreditInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50020"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Products).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Services).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-88.50m).Success.Get(), NonPositiveAmount.Create(-11.50m).Success.Get(), TaxType.Vat13, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.OnCredit).Success.Get(),
                        NegativePayment.Create(NegativeAmount.Create(-100m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    ),
                    counterPart: InvoiceParty.Create(country, TaxpayerIdentificationNumber.Create(country, "090701900").Success.Get(), address: new Address(postalCode: NonEmptyString.CreateUnsafe("12"), city: NonEmptyString.CreateUnsafe("City"))).Success.Get()
                ).Success.Get())
            }).Get();
        }
    }
}
