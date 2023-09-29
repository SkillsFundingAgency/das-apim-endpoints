using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Profiles.Queries.GetProfiles;

public class GetProfilesByUserTypeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllProfiles(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetProfilesByUserTypeQueryHandler handler,
        GetProfilesByUserTypeQuery query,
        GetProfilesByUserTypeQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetProfiles(It.IsAny<string>(), cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}