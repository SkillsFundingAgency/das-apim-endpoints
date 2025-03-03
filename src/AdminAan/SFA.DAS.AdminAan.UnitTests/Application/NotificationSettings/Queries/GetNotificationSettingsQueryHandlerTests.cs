using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Queries;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.NotificationSettings.Queries
{
    public class GetNotificationSettingsQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_Return_NotificationPreferences(
            GetNotificationsSettingsQuery query,
            [Frozen] Mock<IAanHubRestApiClient> apiClient,
            [Frozen] GetNotificationsSettingsQueryHandler handler,
            GetMemberResponse apiResponse)
        {
            apiClient
                .Setup(x => x.GetMember(query.MemberId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ReceiveNotifications.Should().Be(apiResponse.ReceiveNotifications);
        }

    }
}
