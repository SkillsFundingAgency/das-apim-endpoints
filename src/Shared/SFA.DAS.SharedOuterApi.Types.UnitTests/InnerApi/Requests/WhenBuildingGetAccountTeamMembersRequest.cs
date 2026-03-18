using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetAccountTeamMembersRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(long accountId)
    {
        var actual = new GetAccountTeamMembersRequest(accountId);

        actual.GetAllUrl.Should().Be($"api/accounts/internal/{accountId}/users");
    }
}