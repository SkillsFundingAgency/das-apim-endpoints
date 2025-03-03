using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingAcknowledgeEmployerRequestsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(AcknowledgeEmployerRequestsData data)
        {
            var actual = new AcknowledgeEmployerRequestsRequest(data);

            actual.PostUrl.Should().Be($"api/providers/{data.Ukprn}/employer-requests/acknowledge");
            actual.Data.GetType().GetProperty("EmployerRequestIds")!.GetValue(actual.Data, null).Should().BeEquivalentTo(data.EmployerRequestIds);
            actual.Data.GetType().GetProperty("Ukprn")!.GetValue(actual.Data, null).Should().Be(data.Ukprn);
        }
    }
}
