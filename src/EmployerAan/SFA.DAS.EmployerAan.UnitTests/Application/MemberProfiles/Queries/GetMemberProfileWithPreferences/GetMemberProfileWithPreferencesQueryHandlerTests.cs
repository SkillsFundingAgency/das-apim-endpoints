using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
public class GetMemberProfileWithPreferencesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllProfiles(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetMemberProfileWithPreferencesQueryHandler handler,
        GetMemberProfileWithPreferencesQuery query,
        GetMemberProfileWithPreferencesQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetMemberProfileWithPreferences(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}