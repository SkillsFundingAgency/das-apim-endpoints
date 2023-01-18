using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Funding.InnerApi.Requests.ProviderEarnings;

namespace SFA.DAS.Funding.UnitTests.InnerApi.Requests.ProviderEarnings
{
    public class WhenBuildingTheGetProviderEarningsSummaryRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(long ukprn)
        {
            var actual = new GetProviderEarningsSummaryRequest(ukprn);

            actual.GetUrl.Should().Be($"{ukprn}/summary");
        }
    }
}