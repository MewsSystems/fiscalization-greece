using Mews.Fiscalization.Greece.Model;
using Mews.Fiscalization.Greece.Model.Types;
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
            UserVatNumber = Environment.GetEnvironmentVariable("user_vat_number") ?? "800356135";
        }

        public static IEnumerable<object[]> GetInvoices()
        {
            var invoices = new List<object>
            {
                RetailSalesReceiptForCustomer(),
                SalesInvoiceForCompany(),
                InvoiceForForeignCompany("CZ", true),
                InvoiceForForeignCompany("US", false),
                SimplifiedInvoiceForCustomer(),
                CreditInvoiceNonAssociated(),
                CreditInvoiceNonAssociatedForForeignCompany("CZ", true),
                CreditInvoiceNonAssociatedForForeignCompany("US", false)
            };
            return invoices.Select(i => new[] { i });
        }

        private static ISequentialEnumerable<Invoice> RetailSalesReceiptForCustomer()
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new RetailSalesReceipt(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NonNegativePayment(new NonNegativeAmount(133.06m), PaymentType.DomesticPaymentsAccountNumber),
                        new NonNegativePayment(new NonNegativeAmount(66.53m), PaymentType.Cash)
                    )
                )
            });
        }

        private static ISequentialEnumerable<Invoice> SalesInvoiceForCompany()
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new SalesInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.DomesticPaymentsAccountNumber)
                    ),
                    counterpart: new LocalInvoiceParty(new GreekTaxIdentifier("090701900"))
                )
            });
        }

        private static ISequentialEnumerable<Invoice> InvoiceForForeignCompany(string countryCode, bool isWithinEU)
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new SalesInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.Vat0, RevenueType.Products, VatExemptionType.VatIncludedArticle43),
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.Vat0, RevenueType.Services, VatExemptionType.VatIncludedArticle43),
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.Vat0, RevenueType.Other, VatExemptionType.VatIncludedArticle43)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.DomesticPaymentsAccountNumber)
                    ),
                    counterpart: new ForeignInvoiceParty(new Model.Types.Country(new CountryCode(countryCode), isWithinEU: isWithinEU), new NonEmptyString("12348765"), new NonNegativeInt(0), "Name", new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            });
        }

        private static ISequentialEnumerable<Invoice> SimplifiedInvoiceForCustomer()
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new SimplifiedInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(20.50m), new NonNegativeAmount(10.50m), TaxType.Vat13, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(20.50m), new NonNegativeAmount(10.50m), TaxType.Vat13, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(20.50m), new NonNegativeAmount(10.50m), TaxType.Vat13, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NonNegativePayment(new NonNegativeAmount(31m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(31m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(31m), PaymentType.DomesticPaymentsAccountNumber)
                    )
                )
            });
        }

        private static ISequentialEnumerable<Invoice> CreditInvoiceNonAssociated()
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new CreditInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Products),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Services),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.Cash),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.OnCredit),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.DomesticPaymentsAccountNumber)
                    ),
                    counterpart: new LocalInvoiceParty(new GreekTaxIdentifier("090701900"), address: new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            });
        }

        private static ISequentialEnumerable<Invoice> CreditInvoiceNonAssociatedForForeignCompany(string countryCode, bool isWithinEU)
        {
            return SequentialEnumerableStartingWithOne.FromPreordered(new List<Invoice>
            {
                new CreditInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Products),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Services),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NonPositiveAmount(-11.50m), TaxType.Vat13, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.Cash),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.OnCredit),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.DomesticPaymentsAccountNumber)
                    ),
                    counterpart: new ForeignInvoiceParty(new Model.Types.Country(new CountryCode(countryCode), isWithinEU: isWithinEU), new NonEmptyString("12348765"), name: "Name", address: new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            });
        }
    }
}
