using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Regions.Queries.GetRegions;

public class GetRegionsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllRegions(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetRegionsQueryHandler sut,
        GetRegionsQuery query,
        GetRegionsQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}