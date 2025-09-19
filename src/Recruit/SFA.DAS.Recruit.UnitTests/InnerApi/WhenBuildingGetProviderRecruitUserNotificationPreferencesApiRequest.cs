using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;

namespace SFA.DAS.Recruit.UnitTests.InnerApi;

public class WhenBuildingGetProviderRecruitUserNotificationPreferencesApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built(int ukprn, NotificationTypes notificationType)
    {
        var actual = new GetProviderRecruitUserNotificationPreferencesApiRequest(ukprn, notificationType);
        
        actual.GetAllUrl.Should().Be($"api/user/by/ukprn/{ukprn}?notificationType={notificationType}");
    }

    [Test, AutoData]
    public void Then_The_Request_Is_Built_When_NotificationType_Is_Null(int ukprn)
    {
        var actual = new GetProviderRecruitUserNotificationPreferencesApiRequest(ukprn);

        actual.GetAllUrl.Should().Be($"api/user/by/ukprn/{ukprn}");
    }
}