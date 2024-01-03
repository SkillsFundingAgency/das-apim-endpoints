using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.LeavingReasons.Queries.GetLeavingReasons;
public class GetLeavingReasonsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAllLeavingReasons(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetLeavingReasonsQueryHandler handler,
        GetLeavingReasonsQuery query,
        GetLeavingReasonsQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetLeavingReasons(cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.Should().BeEquivalentTo(expected.LeavingCategories);
    }
}