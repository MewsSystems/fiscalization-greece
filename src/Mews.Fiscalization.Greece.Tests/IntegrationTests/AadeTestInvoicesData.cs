using Mews.Fiscalization.Greece.Model;
using Mews.Fiscalization.Greece.Model.Collections;
using Mews.Fiscalization.Greece.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;

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
                InvoiceForForeignCompany("CZ", true),
                InvoiceForForeignCompany("US", false),
                SimplifiedInvoiceForCustomer(),
                CreditInvoiceNonAssociated(),
                CreditInvoiceNonAssociatedForForeignCompany("CZ", true),
                CreditInvoiceNonAssociatedForForeignCompany("US", false)
            };
            return invoices.Select(i => new [] { i });
        }

        private static ISequentialEnumerable<Invoice> RetailSalesReceiptForCustomer()
        {
            return SequentialEnumerable.FromPreordered(
                new RetailSalesReceipt(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(53.65m), new NonNegativeAmount(12.88m), TaxType.Vat6, RevenueType.Other)
                    },
                    payments: new List<NonNegativePayment>
                    {
                        new NonNegativePayment(new NonNegativeAmount(133.06m), PaymentType.DomesticPaymentsAccountNumber),
                        new NonNegativePayment(new NonNegativeAmount(66.53m), PaymentType.Cash)
                    }
                )
            );
        }

        private static ISequentialEnumerable<Invoice> SalesInvoiceForCompany()
        {
            return SequentialEnumerable.FromPreordered(
                new SalesInvoice(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Other)
                    },
                    payments: new List<NonNegativePayment>
                    {
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.DomesticPaymentsAccountNumber)
                    },
                    counterpart: new LocalCounterpart(new GreekTaxIdentifier("090701900"))
                )
            );
        }

        private static ISequentialEnumerable<Invoice> InvoiceForForeignCompany(string countryCode, bool isWithinEU)
        {
            return SequentialEnumerable.FromPreordered(
                new SalesInvoice(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.WithoutVat, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.WithoutVat, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(100m), new NonNegativeAmount(0m), TaxType.WithoutVat, RevenueType.Other)
                    },
                    payments: new List<NonNegativePayment>
                    {
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.DomesticPaymentsAccountNumber)
                    },
                    counterpart: new Counterpart(new Country(new CountryCode(countryCode), isWithinEU: isWithinEU), new NonEmptyString("12348765"), new NonNegativeInt(0), "Name", new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            );
        }

        private static ISequentialEnumerable<Invoice> SimplifiedInvoiceForCustomer()
        {
            return SequentialEnumerable.FromPreordered(
                new SimplifiedInvoice(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NonNegativeRevenue>
                    {
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Products),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Services),
                        new NonNegativeRevenue(new NonNegativeAmount(88.50m), new NonNegativeAmount(11.50m), TaxType.Vat13, RevenueType.Other)
                    },
                    payments: new List<NonNegativePayment>
                    {
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.Cash),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.OnCredit),
                        new NonNegativePayment(new NonNegativeAmount(100m), PaymentType.DomesticPaymentsAccountNumber)
                    }
                )
            );
        }

        private static ISequentialEnumerable<Invoice> CreditInvoiceNonAssociated()
        {
            return SequentialEnumerable.FromPreordered(
                new CreditInvoice(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NegativeRevenue>
                    {
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Products),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Services),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Other)
                    },
                    payments: new List<NegativePayment>
                    {
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.Cash),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.OnCredit),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.DomesticPaymentsAccountNumber)
                    },
                    counterpart: new LocalCounterpart(new GreekTaxIdentifier("090701900"), address: new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            );
        }

        private static ISequentialEnumerable<Invoice> CreditInvoiceNonAssociatedForForeignCompany(string countryCode, bool isWithinEU)
        {
            return SequentialEnumerable.FromPreordered(
                new CreditInvoice(
                    issuer: new LocalCounterpart(new GreekTaxIdentifier(UserVatNumber)),
                    header: new InvoiceHeader(new LimitedString1to50("0"), new LimitedString1to50("50020"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: new List<NegativeRevenue>
                    {
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Products),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Services),
                        new NegativeRevenue(new NegativeAmount(-88.50m), new NegativeAmount(-11.50m), TaxType.Vat13, RevenueType.Other)
                    },
                    payments: new List<NegativePayment>
                    {
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.Cash),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.OnCredit),
                        new NegativePayment(new NegativeAmount(-100m), PaymentType.DomesticPaymentsAccountNumber)
                    },
                    counterpart: new Counterpart(new Country(new CountryCode(countryCode), isWithinEU: isWithinEU), new NonEmptyString("12348765"), name: "Name", address: new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            );
        }
    }
}
