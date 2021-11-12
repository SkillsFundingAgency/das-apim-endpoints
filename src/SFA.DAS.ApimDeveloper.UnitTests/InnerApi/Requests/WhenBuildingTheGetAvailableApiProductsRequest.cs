using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAvailableApiProductsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountType)
        {
            var actual = new GetAvailableApiProductsRequest(accountType);

            actual.GetUrl.Should().Be($"api/products?group={accountType}");
        }
    }
}