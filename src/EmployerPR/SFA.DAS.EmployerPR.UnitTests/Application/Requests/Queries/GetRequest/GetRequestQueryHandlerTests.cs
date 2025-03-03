using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Responses;
using SFA.DAS.EmployerPR.UnitTests.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Queries.GetRequest;

public class GetRequestQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_RequestFound_ReturnsRequest(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> apiClientMock,
        GetRequestQueryHandler sut,
        GetRequestQuery query,
        GetRequestResponse expected,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(expected));

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_RequestNotFound_ReturnsNull(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> apiClientMock,
        GetRequestQueryHandler sut,
        GetRequestQuery query,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetNotFoundResponse<GetRequestResponse>());

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }
}
