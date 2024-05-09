using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Providers.Queries.GetPermissionsHas;
public class GetPermissionsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnPermissions(
        GetPermissionsResponse expected,
        CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClient =
            new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();

        var query = new GetPermissionsQuery();

        apiClient.Setup(x => x.Get<GetPermissionsResponse>(It.IsAny<GetPermissionsRequest>()))
            .ReturnsAsync(expected);

        GetPermissionsHandler handler = new GetPermissionsHandler(apiClient.Object);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Operations.Should().BeEquivalentTo(expected.Operations);
    }
}
