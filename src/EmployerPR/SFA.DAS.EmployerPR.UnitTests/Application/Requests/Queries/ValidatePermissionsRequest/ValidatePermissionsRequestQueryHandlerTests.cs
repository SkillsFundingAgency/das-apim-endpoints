using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.UnitTests.Common;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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

    [Test, MoqAutoData]
    public async Task Handle_RequestTypeIsCreateAccount_SetsRequestStatus(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount;
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeTrue();
        actual.Status.Should().Be(getRequestQueryResult.Status);
    }

    [Test]
    [MoqInlineAutoData(HttpStatusCode.NotFound, false)]
    [MoqInlineAutoData(HttpStatusCode.OK, true)]
    public async Task Handle_RequestTypeIsCreateAccount_SetsRequestStatus(
        HttpStatusCode expectedAccountHistoriesStatusCode,
        bool expectedHasEmployerAccount,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), expectedAccountHistoriesStatusCode, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount;
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.HasEmployerAccount.Should().Be(expectedHasEmployerAccount);
    }
}
