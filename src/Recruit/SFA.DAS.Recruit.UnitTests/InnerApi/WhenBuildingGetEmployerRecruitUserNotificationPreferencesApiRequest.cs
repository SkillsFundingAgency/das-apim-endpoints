using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetEmployerRecruitUserNotificationPreferencesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built(long accountId, NotificationTypes notificationType)
    {
        var actual = new GetEmployerRecruitUserNotificationPreferencesApiRequest(accountId, notificationType);
        
        actual.GetAllUrl.Should().Be($"api/user/by/employeraccountid/{accountId}?notificationType={notificationType}");
    }

    [Test, AutoData]
    public void Then_The_Request_Is_Built_When_NotificationType_Is_Null(long accountId)
    {
        var actual = new GetEmployerRecruitUserNotificationPreferencesApiRequest(accountId);

        actual.GetAllUrl.Should().Be($"api/user/by/employeraccountid/{accountId}?notificationType=");
    }
}