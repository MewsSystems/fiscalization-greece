using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model;
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
            Assert.True(response.SendInvoiceResults.IsSuccess);
            Assert.All(response.SendInvoiceResults.Success.Get().Values, result => Assert.True(result.Value.IsSuccess));
        }

        [Fact]
        public async Task ValidNegativeInvoiceWorks()
        {
            // Arrange
            var client = new AadeClient(UserId, UserSubscriptionKey, AadeEnvironment.Sandbox);

            // Act

            // Step 1 - regular invoice
            var country = Countries.GetByCode("GR").Get();
            var taxNumber = TaxpayerIdentificationNumber.Create(country, UserVatNumber).Success.Get();

            var invoices = SequenceStartingWithOne.FromPreordered(new List<Invoice>
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
            });

            var response = await client.SendInvoicesAsync(invoices.Get());

            Assert.True(response.SendInvoiceResults.IsSuccess);
            Assert.All(response.SendInvoiceResults.Success.Get().Values, result => Assert.True(result.Value.IsSuccess));

            // We need to wait some time to allow external system to store the mark from the first call
            await Task.Delay(1000);

            // Step 2 - negative invoice
            var correlatedInvoice = response.SendInvoiceResults.Success.Get().Values.First().Value.Success.InvoiceRegistrationNumber.Value;

            var negativeInvoice = SequenceStartingWithOne.FromPreordered(new List<Invoice>
            {
                new Invoice(CreditInvoice.Create(
                    issuer: InvoiceParty.Create(country, taxNumber).Success.Get(),
                    correlatedInvoice: correlatedInvoice,
                    header: new InvoiceHeader(String1To50.CreateUnsafe("0"), String1To50.CreateUnsafe("50021"), DateTime.Now, currencyCode: CurrencyCode.Create("EUR").Success.Get()),
                    revenueItems: SequenceStartingWithOne.FromPreordered(new List<NegativeRevenue>
                    {
                        NegativeRevenue.Create(NegativeAmount.Create(-53.65m).Success.Get(), NonPositiveAmount.Create(-12.88m).Success.Get(), TaxType.Vat6, RevenueType.Products).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-53.65m).Success.Get(), NonPositiveAmount.Create(-12.88m).Success.Get(), TaxType.Vat6, RevenueType.Services).Success.Get(),
                        NegativeRevenue.Create(NegativeAmount.Create(-53.65m).Success.Get(), NonPositiveAmount.Create(-12.88m).Success.Get(), TaxType.Vat6, RevenueType.Other).Success.Get()
                    }).Get(),
                    payments: NonEmptyEnumerable.Create(
                        NegativePayment.Create(NegativeAmount.Create(-133.06m).Success.Get(), PaymentType.Cash).Success.Get(),
                        NegativePayment.Create(NegativeAmount.Create(-66.53m).Success.Get(), PaymentType.DomesticPaymentsAccountNumber).Success.Get()
                    ),
                    counterPart: InvoiceParty.Create(country, TaxpayerIdentificationNumber.Create(country, "090701900").Success.Get(), NonNegativeInt.CreateUnsafe(0), address: new Address(postalCode: NonEmptyString.CreateUnsafe("12"), city: NonEmptyString.CreateUnsafe("City"))).Success.Get()
                ).Success.Get())
            });

            var negativeInvoiceResponse = await client.SendInvoicesAsync(negativeInvoice.Get());

            // Assert
            Assert.True(negativeInvoiceResponse.SendInvoiceResults.IsSuccess);
            Assert.All(negativeInvoiceResponse.SendInvoiceResults.Success.Get().Values, result => Assert.True(result.Value.IsSuccess));
        }
    }
}
