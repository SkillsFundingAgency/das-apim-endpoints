using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Settings.Responses;
using SFA.DAS.ApprenticeAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnMemberNotificationSettings(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMemberNotificationSettingsQueryHandler handler,
        GetNotificationSettingsApiResponse expectedSettings,
        GetMemberQueryResult expectedMember,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberNotificationSettingsQuery(memberId);
        var expected = new GetMemberNotificationSettingsQueryResult
        {
            MemberNotificationEventFormats = expectedSettings.EventTypes.Select(x => new MemberNotificationEventFormatModel
            {
                EventFormat = x.EventType,
                ReceiveNotifications = x.ReceiveNotifications
            }),
            MemberNotificationLocations = expectedSettings.Locations.Select(x => new MemberNotificationLocationsModel
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Name = x.Name,
                Radius = x.Radius
            }),
            ReceiveMonthlyNotifications = (bool)expectedMember.ReceiveNotifications
        };
        apiClient.Setup(x => x.GetMemberNotificationSettings(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedSettings);
        apiClient.Setup(x => x.GetMember(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedMember);
        var actual = await handler.Handle(query, cancellationToken);
        actual.MemberNotificationLocations.Should().BeEquivalentTo(expected.MemberNotificationLocations);
        actual.ReceiveMonthlyNotifications.Should().Be(expected.ReceiveMonthlyNotifications);
    }
}