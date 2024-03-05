using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRegistrations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetInvitationRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string correlationId)
        {
            var actual = new GetInvitationRequest(correlationId);

            actual.GetUrl.Should().Be($"api/invitations/{correlationId}");
        }
    }
}
