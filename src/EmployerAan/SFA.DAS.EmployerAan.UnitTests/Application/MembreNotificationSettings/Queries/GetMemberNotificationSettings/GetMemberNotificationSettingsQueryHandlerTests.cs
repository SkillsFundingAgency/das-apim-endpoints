using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMember;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MembreNotificationSettings.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnMemberNotificationSettings(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMemberNotificationSettingsQueryHandler handler,
        GetMemberNotificationSettingsQueryResult expectedSettings,
        GetMemberQueryResult expectedMember,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberNotificationSettingsQuery(memberId);
        var expected = new GetMemberNotificationSettingsQueryResult 
        { 
            MemberNotificationEventFormats = expectedSettings.MemberNotificationEventFormats,
            MemberNotificationLocations = expectedSettings.MemberNotificationLocations,
            ReceiveMonthlyNotifications = (bool)expectedMember.ReceiveNotifications
        };
        apiClient.Setup(x => x.GetMemberNotificationSettings(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedSettings);
        apiClient.Setup(x => x.GetMember(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedMember);
        var actual = await handler.Handle(query, cancellationToken);
        actual.MemberNotificationLocations.Should().BeEquivalentTo(expected.MemberNotificationLocations);
        actual.ReceiveMonthlyNotifications.Should().Be(expected.ReceiveMonthlyNotifications);
    }
}
