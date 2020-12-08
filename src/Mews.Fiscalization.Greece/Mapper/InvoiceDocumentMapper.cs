using Mews.Fiscalization.Greece.Model;
using System;
using System.Linq;
using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model.Types;
using TaxType = Mews.Fiscalization.Greece.Model.TaxType;
using System.Text.RegularExpressions;

namespace Mews.Fiscalization.Greece.Mapper
{
    public static class InvoiceDocumentMapper
    {
        private const string GreeceCountryCode = "GR";

        public static Dto.Xsd.InvoicesDoc GetInvoiceDoc(ISequentialEnumerable<Invoice> invoices)
        {
            return new Dto.Xsd.InvoicesDoc
            {
                Invoices = invoices.Values.Select(invoice => GetInvoice(invoice)).ToArray()
            };
        }

        private static Dto.Xsd.Invoice GetInvoice(Invoice invoice)
        {
            return new Dto.Xsd.Invoice
            {
                InvoiceId = invoice.Header.InvoiceIdentifier,
                InvoiceIssuer = GetInvoiceParty(invoice.Issuer),
                InvoiceCounterpart = GetInvoiceParty(invoice.Counterpart),
                InvoiceSummary = GetInvoiceSummary(invoice),
                InvoiceHeader = GetInvoiceHeader(invoice),
                InvoiceDetails = invoice.RevenueItems.Select(invoiceDetail => GetInvoiceDetail(invoice, invoiceDetail)).ToArray(),
                PaymentMethods = invoice.Payments?.Select(paymentMethod => new Dto.Xsd.PaymentMethod
                {
                    Amount = Math.Abs(paymentMethod.Amount.Value),
                    PaymentMethodType = MapPaymentMethodType(paymentMethod.PaymentType)
                }).ToArray()
            };
        }

        private static Dto.Xsd.InvoiceParty GetInvoiceParty(InvoiceParty counterpart)
        {
            if (counterpart != null)
            {
                return new Dto.Xsd.InvoiceParty
                {
                    Country = (Dto.Xsd.Country)Enum.Parse(typeof(Dto.Xsd.Country), counterpart.Country.Code.Value, true),
                    Branch = counterpart.Branch.Value,
                    Name = counterpart.Name,
                    VatNumber = counterpart.TaxIdentifier.Value,
                    Address = GetAddress(counterpart.Address)
                };
            }

            return null;
        }

        private static Dto.Xsd.Address GetAddress(Address address)
        {
            if (address != null)
            {
                return new Dto.Xsd.Address
                {
                    City = address.City.Value,
                    Number = address.Number,
                    PostalCode = address.PostalCode.Value,
                    Street = address.Street
                };
            }

            return null;
        }

        private static Dto.Xsd.InvoiceHeader GetInvoiceHeader(Invoice invoice)
        {
            var invoiceHeader = new Dto.Xsd.InvoiceHeader
            {
                InvoiceType = MapInvoiceType(invoice),
                IssueDate = invoice.Header.InvoiceIssueDate,
                SerialNumber = invoice.Header.InvoiceSerialNumber.Value,
                Series = invoice.Header.InvoiceSeries.Value,
                CurrencySpecified = invoice.Header.CurrencyCode.IsNotNull(),
                ExchangeRateSpecified = invoice.Header.ExchangeRate.IsNotNull(),
                ExchangeRate = invoice.Header.ExchangeRate?.Value ?? 0,
                CorrelatedInvoicesSpecified = invoice.CorrelatedInvoice.IsNotNull(),
                CorrelatedInvoices = invoice.CorrelatedInvoice ?? 0
            };

            if (invoice.Header.CurrencyCode.IsNotNull())
            {
                invoiceHeader.Currency = (Dto.Xsd.Currency)Enum.Parse(typeof(Dto.Xsd.Currency), invoice.Header.CurrencyCode.Value, true);
            }

            return invoiceHeader;
        }

        private static Dto.Xsd.InvoiceDetail GetInvoiceDetail(Invoice invoice, IIndexedItem<Revenue> indexedRevenueItem)
        {
            var revenueItem = indexedRevenueItem.Value;
            var invoiceDetail = new Dto.Xsd.InvoiceDetail
            {
                LineNumber = indexedRevenueItem.Index,
                NetValue = Math.Abs(revenueItem.NetValue.Value),
                VatAmount = Math.Abs(revenueItem.VatValue.Value),
                VatCategory = MapVatCategory(revenueItem.TaxType),
                IncomeClassification = new[] { GetIncomeClassification(invoice, revenueItem) }
            };

            if (revenueItem.VatExemption.HasValue)
            {
                invoiceDetail.VatExemptionCategory = MapVatExemptionCategory(revenueItem.VatExemption.Value);
                invoiceDetail.VatExemptionCategorySpecified = true;
            }

            return invoiceDetail;
        }

        private static Dto.Xsd.InvoiceSummary GetInvoiceSummary(Invoice invoice)
        {
            var invoiceSummary = new Dto.Xsd.InvoiceSummary
            {
                TotalNetValue = Math.Abs(invoice.RevenueItems.Sum(x => x.Value.NetValue.Value)),
                TotalVatAmount = Math.Abs(invoice.RevenueItems.Sum(x => x.Value.VatValue.Value))
            };

            invoiceSummary.IncomeClassification = invoice.RevenueItems.GroupBy(
                keySelector: m => m.Value.RevenueType,
                resultSelector: (key, revenueItems) => new Dto.Xsd.IncomeClassification
                {
                    ClassificationCategory = MapRevenueClassification(invoice, key).Category,
                    ClassificationType = MapRevenueClassification(invoice, key).Type,
                    Amount = Math.Abs(revenueItems.Sum(i => i.Value.NetValue.Value))
                }
            ).ToArray();


            invoiceSummary.TotalGrossValue = invoiceSummary.TotalNetValue + invoiceSummary.TotalVatAmount + invoiceSummary.TotalOtherTaxesAmount;

            return invoiceSummary;
        }

        private static Dto.Xsd.IncomeClassification GetIncomeClassification(Invoice invoice, Revenue revenue)
        {
            var revenueClassification = MapRevenueClassification(invoice, revenue.RevenueType);

            return new Dto.Xsd.IncomeClassification
            {
                Amount = Math.Abs(revenue.NetValue.Value),
                ClassificationCategory = revenueClassification.Category,
                ClassificationType = revenueClassification.Type
            };
        }

        private static Dto.Xsd.InvoiceType MapInvoiceType(Invoice invoice)
        {
            if (invoice is CreditInvoice)
            {
                if (invoice.CorrelatedInvoice.IsNotNull())
                {
                    return Dto.Xsd.InvoiceType.CreditInvoiceAssociated;
                }
                return Dto.Xsd.InvoiceType.CreditInvoiceNonAssociated;
            }

            if (invoice is RetailSalesReceipt)
            {
                return Dto.Xsd.InvoiceType.RetailSalesReceipt;
            }

            if (invoice is SimplifiedInvoice)
            {
                return Dto.Xsd.InvoiceType.SimplifiedInvoice;
            }

            if (invoice is SalesInvoice)
            {
                Model.Types.Country country = invoice.Counterpart.Country;
                if (country.Code.Value == GreeceCountryCode)
                {
                    return Dto.Xsd.InvoiceType.SalesInvoice;
                }
                else if (country.IsWithinEU)
                {
                    return Dto.Xsd.InvoiceType.SalesInvoiceIntraCommunitySupplies;
                }
                return Dto.Xsd.InvoiceType.SalesInvoiceThirdCountrySupplies;
            }

            throw new ArgumentException($"Cannot map invoice to {nameof(Dto.Xsd.InvoiceType)}.");
        }

        private static (Dto.Xsd.IncomeClassificationCategory Category, Dto.Xsd.IncomeClassificationType Type) MapRevenueClassification(
            Invoice invoice,
            RevenueType revenueType)
        {
            if (revenueType == RevenueType.Products)
            {
                return (Dto.Xsd.IncomeClassificationCategory.ProductSaleIncome, GetGoodsAndServicesClassificationType());
            }
            else if (revenueType == RevenueType.Services)
            {
                return (Dto.Xsd.IncomeClassificationCategory.ProvisionOfServicesIncome, GetGoodsAndServicesClassificationType());
            }
            else if (revenueType == RevenueType.Other)
            {
                if (invoice is SalesInvoice)
                {
                    return (Dto.Xsd.IncomeClassificationCategory.OtherIncomeAndProfits, Dto.Xsd.IncomeClassificationType.OtherOrdinaryIncome);
                }
                else if (invoice is CreditInvoice)
                {
                    if (invoice.CorrelatedInvoice.IsNotNull())
                    {
                        return (Dto.Xsd.IncomeClassificationCategory.OtherIncomeAndProfits, Dto.Xsd.IncomeClassificationType.OtherOrdinaryIncome);
                    }
                }

                return (Dto.Xsd.IncomeClassificationCategory.OtherIncomeAndProfits, Dto.Xsd.IncomeClassificationType.OtherSalesOfGoodsAndServices);
            }

            throw new ArgumentException($"Cannot map revenue type to a pair of {nameof(Dto.Xsd.IncomeClassificationCategory)} and {nameof(Dto.Xsd.IncomeClassificationType)}.");

            Dto.Xsd.IncomeClassificationType GetGoodsAndServicesClassificationType()
            {
                if (invoice is SalesInvoice)
                {
                    Model.Types.Country country = invoice.Counterpart.Country;
                    if (country.Code.Value == GreeceCountryCode)
                    {
                        return Dto.Xsd.IncomeClassificationType.OtherSalesOfGoodsAndServices;
                    }
                    else if (country.IsWithinEU)
                    {
                        return Dto.Xsd.IncomeClassificationType.IntraCommunityForeignSalesOfGoodsAndServices;
                    }
                    return Dto.Xsd.IncomeClassificationType.ThirdCountryForeignSalesOfGoodsAndServices;
                }
                else if (invoice is CreditInvoice)
                {
                    return Dto.Xsd.IncomeClassificationType.OtherSalesOfGoodsAndServices;
                }

                return Dto.Xsd.IncomeClassificationType.RetailSalesOfGoodsAndServicesPrivateClientele;
            }
        }

        private static Dto.Xsd.VatCategory MapVatCategory(TaxType taxType)
        {
            switch (taxType)
            {
                case TaxType.Vat24:
                    return Dto.Xsd.VatCategory.Vat24;
                case TaxType.Vat13:
                    return Dto.Xsd.VatCategory.Vat13;
                case TaxType.Vat6:
                    return Dto.Xsd.VatCategory.Vat6;
                case TaxType.Vat0:
                    return Dto.Xsd.VatCategory.Vat0;
                case TaxType.WithoutVat:
                    return Dto.Xsd.VatCategory.WithoutVat;
                default:
                    throw new ArgumentException($"Cannot map TaxType {taxType} to {nameof(Dto.Xsd.VatCategory)}.");
            }
        }

        private static Dto.Xsd.PaymentMethodType MapPaymentMethodType(PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.DomesticPaymentsAccountNumber:
                    return Dto.Xsd.PaymentMethodType.DomesticPaymentsAccountNumber;
                case PaymentType.ForeignMethodsAccountNumber:
                    return Dto.Xsd.PaymentMethodType.ForeignMethodsAccountNumber;
                case PaymentType.Check:
                    return Dto.Xsd.PaymentMethodType.Check;
                case PaymentType.OnCredit:
                    return Dto.Xsd.PaymentMethodType.OnCredit;
                case PaymentType.Cash:
                    return Dto.Xsd.PaymentMethodType.Cash;
                default:
                    throw new ArgumentException($"Cannot map PaymentType {paymentType} to {nameof(Dto.Xsd.PaymentMethodType)}.");
            }
        }

        private static Dto.Xsd.VatExemptionCategory MapVatExemptionCategory(VatExemptionType vatExemption)
        {
            switch (vatExemption)
            {
                case VatExemptionType.VatIncludedArticle43:
                    return Dto.Xsd.VatExemptionCategory.VatIncludedArticle43;
                case VatExemptionType.VatIncludedArticle44:
                    return Dto.Xsd.VatExemptionCategory.VatIncludedArticle44;
                case VatExemptionType.VatIncludedArticle45:
                    return Dto.Xsd.VatExemptionCategory.VatIncludedArticle45;
                case VatExemptionType.VatIncludedArticle46:
                    return Dto.Xsd.VatExemptionCategory.VatIncludedArticle46;
                case VatExemptionType.WithoutVatArticle13:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle13;
                case VatExemptionType.WithoutVatArticle14:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle14;
                case VatExemptionType.WithoutVatArticle16:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle16;
                case VatExemptionType.WithoutVatArticle19:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle19;
                case VatExemptionType.WithoutVatArticle22:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle22;
                case VatExemptionType.WithoutVatArticle24:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle24;
                case VatExemptionType.WithoutVatArticle25:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle25;
                case VatExemptionType.WithoutVatArticle26:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle26;
                case VatExemptionType.WithoutVatArticle27:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle27;
                case VatExemptionType.WithoutVatArticle271CSeagoingVessels:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle271CSeagoingVessels;
                case VatExemptionType.WithoutVatArticle27SeagoingVessels:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle27SeagoingVessels;
                case VatExemptionType.WithoutVatArticle28:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle28;
                case VatExemptionType.WithoutVatArticle3:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle3;
                case VatExemptionType.WithoutVatArticle39:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle39;
                case VatExemptionType.WithoutVatArticle39A:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle39A;
                case VatExemptionType.WithoutVatArticle40:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle40;
                case VatExemptionType.WithoutVatArticle41:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle41;
                case VatExemptionType.WithoutVatArticle47:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle47;
                case VatExemptionType.WithoutVatArticle5:
                    return Dto.Xsd.VatExemptionCategory.WithoutVatArticle5;
                default:
                    throw new ArgumentException($"Cannot map VatExemption {vatExemption} to Dto.Xsd.{nameof(Dto.Xsd.VatExemptionCategory)}.");
            }
        }
    }
}
