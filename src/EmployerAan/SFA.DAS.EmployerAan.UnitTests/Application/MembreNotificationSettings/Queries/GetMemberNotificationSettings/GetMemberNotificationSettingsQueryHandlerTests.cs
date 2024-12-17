using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;
using SFA.DAS.EmployerAan.Application.MemberNotificationLocations.Queries;
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

        GetMemberNotificationLocationsQueryResult expectedLocations,
        GetMemberNotificationEventFormatsQueryResult expectedEventFormats,
        GetMemberQueryResult expectedMember,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberNotificationSettingsQuery(memberId);
        var expected = new GetMemberNotificationSettingsQueryResult 
        { 
            MemberNotificationEventFormats = expectedEventFormats.MemberNotificationEventFormats,
            MemberNotificationLocations = expectedLocations.MemberNotificationLocations,
            ReceiveMonthlyNotifications = (bool)expectedMember.ReceiveNotifications
        };
        apiClient.Setup(x => x.GetMemberNotificationEventFormat(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedEventFormats);
        apiClient.Setup(x => x.GetMemberNotificationLocations(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedLocations);
        apiClient.Setup(x => x.GetMember(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expectedMember);
        var actual = await handler.Handle(query, cancellationToken);
        actual.MemberNotificationLocations.Should().BeEquivalentTo(expected.MemberNotificationLocations);
        actual.ReceiveMonthlyNotifications.Should().Be(expected.ReceiveMonthlyNotifications);
    }
}
