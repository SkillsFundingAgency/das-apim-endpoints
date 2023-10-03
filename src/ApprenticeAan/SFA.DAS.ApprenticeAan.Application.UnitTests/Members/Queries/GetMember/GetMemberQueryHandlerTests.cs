using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Members.Queries.GetMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Members.Queries.GetMember;
public class GetMemberQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnsMember(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetMemberQuery query,
        GetMemberQueryHandler handler,
        GetMemberQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(x => x.GetMember(query.MemberId, cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().Be(expected);
        apiClientMock.Verify(x => x.GetMember(query.MemberId, new CancellationToken()), Times.Once());
    }
}
