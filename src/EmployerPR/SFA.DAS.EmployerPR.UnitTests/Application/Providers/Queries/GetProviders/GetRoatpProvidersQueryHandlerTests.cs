using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetRoatpProviders;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Providers.Queries.GetProviders;
public class GetRoatpProvidersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IRoatpV2TrainingProviderService> apiClient,
        GetRoatpProvidersQueryHandler handler,
        GetProvidersResponse expected,
        CancellationToken cancellationToken)
    {
        var query = new GetRoatpProvidersQuery();
        apiClient.Setup(x => x.GetProviders(cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected.RegisteredProviders.Select(provider => (RoatpProvider)provider));
    }
}
