using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Settings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnMemberNotificationSettings(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMemberNotificationSettingsQueryHandler handler,
        GetNotificationSettingsApiResponse expectedSettings,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberNotificationSettingsQuery(memberId);
        var expected = new GetMemberNotificationSettingsQueryResult 
        { 
            MemberNotificationEventFormats = expectedSettings.EventTypes.Select(x => new GetMemberNotificationSettingsQueryResult.EventType
            {
                EventFormat = x.EventType,
                ReceiveNotifications = x.ReceiveNotifications
            }).ToList(),
            MemberNotificationLocations = expectedSettings.Locations.Select(x => new GetMemberNotificationSettingsQueryResult.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }),
            ReceiveMonthlyNotifications = expectedSettings.ReceiveNotifications
        };
        apiClient.Setup(x => x.GetMemberNotificationSettings(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedSettings);
        var actual = await handler.Handle(query, cancellationToken);
        actual.MemberNotificationLocations.Should().BeEquivalentTo(expected.MemberNotificationLocations);
        actual.ReceiveMonthlyNotifications.Should().Be(expected.ReceiveMonthlyNotifications);
    }
}
