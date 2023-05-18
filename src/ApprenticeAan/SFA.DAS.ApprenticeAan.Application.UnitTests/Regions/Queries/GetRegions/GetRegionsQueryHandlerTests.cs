using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Regions.Queries.GetRegions;

public class GetRegionsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAllRegions(
        GetRegionsQuery query,
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        [Frozen(Matching.ImplementedInterfaces)]
        GetRegionsQueryHandler handler,
        GetRegionsQueryResult expected)
    {
        apiClient.Setup(x => x.GetRegions()).ReturnsAsync(expected);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().Be(expected);
    }
}