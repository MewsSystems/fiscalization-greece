using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model;
using System;
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
    }
}
