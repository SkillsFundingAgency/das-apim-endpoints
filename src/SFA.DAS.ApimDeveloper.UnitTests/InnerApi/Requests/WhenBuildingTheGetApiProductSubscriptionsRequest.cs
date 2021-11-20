using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;

namespace SFA.DAS.ApimDeveloper.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetApiProductSubscriptionsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string accountIdentifier)
        {
            var actual = new GetApiProductSubscriptionsRequest(accountIdentifier);

            actual.GetUrl.Should().Be($"api/subscription/{accountIdentifier}");
        }
    }
}