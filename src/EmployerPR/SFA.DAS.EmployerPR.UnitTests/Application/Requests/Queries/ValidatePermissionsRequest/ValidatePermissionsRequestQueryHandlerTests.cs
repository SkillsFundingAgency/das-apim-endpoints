using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.UnitTests.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Queries.ValidatePermissionsRequest;

public class ValidatePermissionsRequestQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_RequestNotFound_SetsIsRequestValidToFalse(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> clientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        CancellationToken cancellationToken)
    {
        clientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetNotFoundResponse<GetRequestQueryResult>());

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeFalse();
    }

    [Test]
    [MoqInlineAutoData(RequestType.AddAccount)]
    [MoqInlineAutoData(RequestType.Permission)]
    public async Task Handle_RequestTypeIsNotCreateAccount_SetsIsRequestValidToFalse(
        RequestType requestType,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> clientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        getRequestQueryResult.RequestType = requestType;
        clientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeFalse();
    }

    [Test, MoqInlineAutoData]
    public async Task Handle_RequestTypeIsCreateAccount_SetsRequestStatus(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> clientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        getRequestQueryResult.RequestType = RequestType.CreateAccount;
        clientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeTrue();
        actual.Status.Should().Be(getRequestQueryResult.Status);
    }
}
