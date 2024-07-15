using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Providers.Queries.GetPermissionsHas;
public class GetPermissionsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnPermissions(
        GetPermissionsResponse expected,
        GetPermissionsQuery query,
        CancellationToken cancellationToken
    )
    {
        Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient =
            new Mock<IProviderRelationshipsApiRestClient>();

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetPermissions(
                It.IsAny<long>(),
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            )
        )
        .ReturnsAsync(expected);

        GetPermissionsHandler handler = new GetPermissionsHandler(providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Operations.Should().BeEquivalentTo(expected.Operations);
    }
}