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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
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
        getRequestQueryResult.RequestType = requestType.ToString();
        clientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_RequestTypeIsCreateAccount_SetsRequestStatusAndEmployerContactEmail(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> tprApiClientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), HttpStatusCode.NotFound, null));

        tprApiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE && r.Aorn == getRequestQueryResult.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(Enumerable.Empty<PensionRegulatorOrganisation>(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount.ToString();
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        var actual = await sut.Handle(query, cancellationToken);

        actual.IsRequestValid.Should().BeTrue();
        actual.Status.Should().Be(getRequestQueryResult.Status);
        actual.EmployerContactEmail.Should().Be(getRequestQueryResult.EmployerContactEmail);
    }

    [Test]
    [MoqInlineAutoData(HttpStatusCode.NotFound, false)]
    [MoqInlineAutoData(HttpStatusCode.OK, true)]
    public async Task Handle_SetsHasEmployerAccount(
        HttpStatusCode expectedAccountHistoriesStatusCode,
        bool expectedHasEmployerAccount,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> tprApiClientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        tprApiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE && r.Aorn == getRequestQueryResult.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(Enumerable.Empty<PensionRegulatorOrganisation>(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount.ToString();
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        /// key setup
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), expectedAccountHistoriesStatusCode, null));

        /// action
        var actual = await sut.Handle(query, cancellationToken);

        actual.HasEmployerAccount.Should().Be(expectedHasEmployerAccount);
    }


    [Test, MoqAutoData]
    public async Task Handle_OrgNotFoundInTpr_SetsHasValidaPayeToFalse(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> tprApiClientMock,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount.ToString();
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        /// key setup
        tprApiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE && r.Aorn == getRequestQueryResult.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(Enumerable.Empty<PensionRegulatorOrganisation>(), HttpStatusCode.NotFound, null));

        /// action
        var actual = await sut.Handle(query, cancellationToken);

        actual.HasValidaPaye.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_OrgFoundInTprWithStatus_SetsHasValidaPayeToFalse(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> tprApiClientMock,
        PensionRegulatorOrganisation pensionRegulatorOrganisation,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount.ToString();
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        /// key setup
        pensionRegulatorOrganisation.Status = "has some status";
        tprApiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE && r.Aorn == getRequestQueryResult.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>([pensionRegulatorOrganisation], HttpStatusCode.OK, null));

        /// action
        var actual = await sut.Handle(query, cancellationToken);

        actual.HasValidaPaye.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Handle_OrgFoundInTprWithEmptyStatus_SetsHasValidaPayeToTrue(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> prApiClientMock,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClientMock,
        [Frozen] Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> tprApiClientMock,
        PensionRegulatorOrganisation pensionRegulatorOrganisation,
        ValidatePermissionsRequestQueryHandler sut,
        ValidatePermissionsRequestQuery query,
        GetRequestQueryResult getRequestQueryResult,
        CancellationToken cancellationToken)
    {
        accountsApiClientMock.Setup(a => a.GetWithResponseCode<GetAccountHistoriesByPayeResponse>(It.Is<GetAccountHistoriesByPayeRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE))).ReturnsAsync(new ApiResponse<GetAccountHistoriesByPayeResponse>(new(), HttpStatusCode.NotFound, null));

        getRequestQueryResult.RequestType = RequestType.CreateAccount.ToString();
        prApiClientMock.Setup(c => c.GetRequest(query.RequestId, cancellationToken)).ReturnsAsync(RestEaseResponseBuilder.GetOkResponse(getRequestQueryResult));

        /// key setup
        pensionRegulatorOrganisation.Status = string.Empty;
        tprApiClientMock.Setup(a => a.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == getRequestQueryResult.EmployerPAYE && r.Aorn == getRequestQueryResult.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>([pensionRegulatorOrganisation], HttpStatusCode.OK, null));

        /// action
        var actual = await sut.Handle(query, cancellationToken);

        actual.HasValidaPaye.Should().BeTrue();
    }
}
