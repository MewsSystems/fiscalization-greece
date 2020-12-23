using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model;
using Mews.Fiscalization.Greece.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mews.Fiscalization.Greece.Tests.IntegrationTests
{
    public class AadeClientTests
    {
        private static readonly string UserId = "";
        private static readonly string UserSubscriptionKey = "";
        private static readonly string UserVatNumber = "";

        static AadeClientTests()
        {
            UserId = Environment.GetEnvironmentVariable("user_id") ?? "tashamews1";
            UserSubscriptionKey = Environment.GetEnvironmentVariable("user_subscription_key") ?? "c3cf0906919c4678877ea2d3fb368661";
            UserVatNumber = Environment.GetEnvironmentVariable("user_vat_number") ?? "800356135";
        }


        [Fact]
        public async Task CheckUserCredentials()
        {
            // Arrange
            var client = new AadeClient(UserId, UserSubscriptionKey, AadeEnvironment.Sandbox);

            // Act
            var response = await client.CheckUserCredentialsAsync();

            // Assert
            Assert.True(response.IsSuccess);
            Assert.True(response.Success.IsAuthorized);
        }

        [Theory]
        [MemberData(nameof(AadeTestInvoicesData.GetInvoices), MemberType = typeof(AadeTestInvoicesData))]
        public async Task ValidInvoicesWork(ISequenceStartingWithOne<Invoice> invoices)
        {
            // Arrange
            var client = new AadeClient(UserId, UserSubscriptionKey, AadeEnvironment.Sandbox);

            // Act
            var response = await client.SendInvoicesAsync(invoices);

            // Assert
            Assert.NotEmpty(response.SendInvoiceResults.Success.Get().Values);
            Assert.True(response.SendInvoiceResults.Success.Get().Values.Single().Value.IsSuccess);
        }

        [Fact]
        public async Task ValidNegativeInvoiceWorks()
        {
            // Arrange
            var client = new AadeClient(UserId, UserSubscriptionKey, AadeEnvironment.Sandbox);

            // Act

            // Step 1 - regular invoice
            var invoices = SequentialEnumerableStartingWithOne.FromPreordered<Invoice>(new List<Invoice>
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

            var response = await client.SendInvoicesAsync(invoices);

            Assert.NotEmpty(response.SendInvoiceResults);
            Assert.True(response.SendInvoiceResults.Single().Value.IsSuccess);

            // We need to wait some time to allow external system to store the mark from the first call
            await Task.Delay(1000);

            // Step 2 - negative invoice
            var correlatedInvoice = response.SendInvoiceResults.First().Value.Success.InvoiceRegistrationNumber.Value;

            var negativeInvoice = SequentialEnumerableStartingWithOne.FromPreordered<Invoice>(new List<Invoice>
            {
                new CreditInvoice(
                    issuer: new LocalInvoiceParty(new GreekTaxIdentifier(UserVatNumber)),
                    correlatedInvoice: correlatedInvoice,
                    header: new InvoiceHeader(new LimitedString1To50("0"), new LimitedString1To50("50021"), DateTime.Now, currencyCode: new CurrencyCode("EUR")),
                    revenueItems: SequentialEnumerableStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        new NegativeRevenue(new NegativeAmount(-53.65m), new NonPositiveAmount(-12.88m), TaxType.Vat6, RevenueType.Products),
                        new NegativeRevenue(new NegativeAmount(-53.65m), new NonPositiveAmount(-12.88m), TaxType.Vat6, RevenueType.Services),
                        new NegativeRevenue(new NegativeAmount(-53.65m), new NonPositiveAmount(-12.88m), TaxType.Vat6, RevenueType.Other)
                    }),
                    payments: NonEmptyEnumerable.Create(
                        new NegativePayment(new NegativeAmount(-133.06m), PaymentType.Cash),
                        new NegativePayment(new NegativeAmount(-66.53m), PaymentType.DomesticPaymentsAccountNumber)
                    ),
                    counterpart: new LocalInvoiceParty(new GreekTaxIdentifier("090701900"), new NonNegativeInt(0), address: new Address(postalCode: new NonEmptyString("12"), city: new NonEmptyString("City")))
                )
            });

            var negativeInvoiceResponse = await client.SendInvoicesAsync(negativeInvoice);

            // Assert
            Assert.NotEmpty(negativeInvoiceResponse.SendInvoiceResults);
            Assert.True(negativeInvoiceResponse.SendInvoiceResults.Single().Value.IsSuccess);
        }
    }
}
