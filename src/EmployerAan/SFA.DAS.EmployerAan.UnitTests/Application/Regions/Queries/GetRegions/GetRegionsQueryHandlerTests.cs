using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Regions.Queries.GetRegions;

public class GetRegionsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnAllRegions(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetRegionsQueryHandler handler,
        GetRegionsQuery query,
        GetRegionsQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetRegions(cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}