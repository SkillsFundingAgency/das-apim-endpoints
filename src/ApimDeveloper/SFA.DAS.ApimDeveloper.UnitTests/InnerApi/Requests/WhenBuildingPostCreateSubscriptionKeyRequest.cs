using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateSubscriptionKeyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_And_Body_Is_Set(string accountIdentifier, string productId)
        {
            var actual = new PostCreateSubscriptionKeyRequest(accountIdentifier, productId);

            actual.PostUrl.Should().Be("api/subscription");
            var actualData = actual.Data as CreateSubscriptionApiRequest;
            Assert.That(actualData, Is.Not.Null);
            actualData.AccountIdentifier.Should().Be(accountIdentifier);
            actualData.ProductId.Should().Be(productId);
        }
    }
}