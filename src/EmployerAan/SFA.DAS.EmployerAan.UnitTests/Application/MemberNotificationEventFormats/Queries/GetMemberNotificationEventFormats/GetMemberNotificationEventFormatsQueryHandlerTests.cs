using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.MemberNotificationEventFormat.Queries.GetMemberNotificationEventFormats;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnEventFormats(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMemberNotificationEventFormatsQueryHandler handler,
        GetMemberNotificationEventFormatsQueryResult expected,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberNotificationEventFormatsQuery(memberId);
        apiClient.Setup(x => x.GetMemberNotificationEventFormat(It.Is<Guid>(id => id == memberId), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}

