using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetSelectEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(long ukprn, string standardReference)
        {
            var actual = new GetSelectEmployerRequestsRequest(standardReference, ukprn);

            var expected = $"api/providers/{ukprn}/employer-requests/{standardReference}/select";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
