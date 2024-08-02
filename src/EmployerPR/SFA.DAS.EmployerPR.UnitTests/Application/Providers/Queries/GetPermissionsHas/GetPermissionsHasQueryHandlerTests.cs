using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestEase;
using SFA.DAS.EmployerPR.Application.Queries.GetPermissions;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Providers.Queries.GetPermissionsHas;
public class GetPermissionsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnPermissionsAndNames(
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
            .ReturnsAsync(new Response<GetPermissionsResponse>(string.Empty, new(HttpStatusCode.OK), () => expected));

        GetPermissionsHandler handler = new GetPermissionsHandler(providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(query, cancellationToken);
        actual!.Operations.Should().BeEquivalentTo(expected.Operations);
        actual.ProviderName.Should().Be(expected.ProviderName);
        actual.AccountLegalEntityName.Should().Be(expected.AccountLegalEntityName);
    }


    [Test, MoqAutoData]
    public async Task Handle_Recieves404_ReturnsNull(
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
            .ReturnsAsync(new Response<GetPermissionsResponse>(string.Empty, new(HttpStatusCode.NotFound), () => expected)!);

        GetPermissionsHandler handler = new GetPermissionsHandler(providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnexpectedApiResponse_ThrowsException(
        GetPermissionsQuery query,
        CancellationToken cancellationToken)
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
            .ReturnsAsync(new Response<GetPermissionsResponse?>(string.Empty, new(HttpStatusCode.InternalServerError), () => null)!);

        GetPermissionsHandler handler = new GetPermissionsHandler(providerRelationshipsApiRestClient.Object);

        Func<Task> action = () => handler.Handle(query, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}