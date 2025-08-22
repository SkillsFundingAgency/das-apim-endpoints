using SFA.DAS.Recruit.InnerApi.Recruit.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetEmployerRecruitUserNotificationPreferencesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built(long accountId)
    {
        var actual = new GetEmployerRecruitUserNotificationPreferencesApiRequest(accountId);
        
        actual.GetAllUrl.Should().Be($"api/user/by/employeraccountid/{accountId}");
    }
}