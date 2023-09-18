using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.GetMemberProfileWithPreferences;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberProfiles.Queries.GetMemberProfileWithPreferences;

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