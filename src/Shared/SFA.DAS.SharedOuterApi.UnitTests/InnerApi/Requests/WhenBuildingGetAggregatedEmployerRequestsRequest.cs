using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAggregatedEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new GetAggregatedEmployerRequestsRequest();

            var expected = "api/employerrequest/aggregated-employer-requests";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
