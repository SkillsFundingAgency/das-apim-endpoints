using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingSubmitproviderResponseRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(SubmitProviderResponseRequestData data)
        {
            var actual = new SubmitProviderResponseRequest(data);
            actual.PostUrl.Should().Be($"api/providers/{data.Ukprn}/responses");
            actual.Data.GetType().GetProperty("EmployerRequestIds")!.GetValue(actual.Data, null).Should().BeEquivalentTo(data.EmployerRequestIds);
            actual.Data.GetType().GetProperty("Ukprn")!.GetValue(actual.Data, null).Should().Be(data.Ukprn);
            actual.Data.GetType().GetProperty("Email")!.GetValue(actual.Data, null).Should().Be(data.Email);
            actual.Data.GetType().GetProperty("Phone")!.GetValue(actual.Data, null).Should().Be(data.Phone);
            actual.Data.GetType().GetProperty("Website")!.GetValue(actual.Data, null).Should().Be(data.Website);
            actual.Data.GetType().GetProperty("RespondedBy")!.GetValue(actual.Data, null).Should().Be(data.RespondedBy);
            actual.Data.GetType().GetProperty("ContactName")!.GetValue(actual.Data, null).Should().Be(data.ContactName);
        }
    }
}
