using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAggregatedEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(long ukprn)
        {
            var actual = new GetAggregatedEmployerRequestsRequest(ukprn);

            var expected = $"api/providers/{ukprn}/employer-requests/aggregated";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
