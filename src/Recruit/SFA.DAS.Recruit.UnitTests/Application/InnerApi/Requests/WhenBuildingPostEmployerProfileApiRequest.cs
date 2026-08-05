using SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;
using static SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles.PostEmployerProfileApiRequest;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

internal class WhenBuildingPostEmployerProfileApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(long accountLegalEntityId, PostEmployerProfileApiRequestData payload)
    {
        var actual = new PostEmployerProfileApiRequest(accountLegalEntityId, payload);

        actual.PutUrl.Should().Be($"api/employer/profiles/{accountLegalEntityId}");
        actual.Data.Should().Be(payload);
    }
}