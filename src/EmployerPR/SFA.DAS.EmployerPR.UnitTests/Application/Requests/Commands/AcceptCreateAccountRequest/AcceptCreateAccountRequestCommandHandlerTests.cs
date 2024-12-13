using System.Net;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestEase;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptCreateAccountRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;
using SFA.DAS.EmployerPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandHandlerTests
{
    private AcceptCreateAccountRequestCommandHandler _sut;
    private Mock<IProviderRelationshipsApiRestClient> _prApiClientMock;
    private Mock<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>> _tprApiClientMock;
    private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClientMock;
    private Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> _employerProfilesApiMock;
    private GetRequestResponse _permissionRequest;
    private AcceptCreateAccountRequestCommand _command;
    private AcceptCreateAccountRequestCommandResult _result;
    private PostCreateAccountResponse _createAccountResponse;

    [SetUp]
    public async Task Initialize()
    {
        _prApiClientMock = new();
        _tprApiClientMock = new();
        _accountsApiClientMock = new();
        _employerProfilesApiMock = new();

        _sut = new(_prApiClientMock.Object, _tprApiClientMock.Object, _accountsApiClientMock.Object, _employerProfilesApiMock.Object);
        Fixture fixture = new();
        _permissionRequest = fixture.Create<GetRequestResponse>();

        _command = fixture.Create<AcceptCreateAccountRequestCommand>();
        _prApiClientMock.Setup(m => m.GetRequest(_command.RequestId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetRequestResponse?>(null, new HttpResponseMessage(HttpStatusCode.OK), () => _permissionRequest));

        IEnumerable<PensionRegulatorOrganisation> tprResponse = [fixture.Build<PensionRegulatorOrganisation>().With(t => t.Status, TprOrganisationStatus.NotClosed).Create()];
        _tprApiClientMock.Setup(t => t.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == _permissionRequest.EmployerPAYE && r.Aorn == _permissionRequest.EmployerAORN))).ReturnsAsync(new ApiResponse<IEnumerable<PensionRegulatorOrganisation>>(tprResponse, HttpStatusCode.OK, string.Empty));

        _createAccountResponse = fixture.Create<PostCreateAccountResponse>();
        _accountsApiClientMock.Setup(a => a.PostWithResponseCode<CreateAccountRequestBody, PostCreateAccountResponse>(It.Is<PostCreateAccountRequest>(r => true), true)).ReturnsAsync(new ApiResponse<PostCreateAccountResponse>(_createAccountResponse, HttpStatusCode.OK, string.Empty));

        var getUserResponse = fixture.Create<EmployerProfileUsersApiResponse>();
        _employerProfilesApiMock.Setup(e => e.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.Is<GetEmployerUserAccountRequest>(r => r.GetUrl.Contains(_command.UserRef.ToString())))).ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(getUserResponse, HttpStatusCode.OK, null));

        _employerProfilesApiMock.Setup(e => e.PutWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<PutUpsertEmployerUserAccountRequest>())).ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(getUserResponse, HttpStatusCode.OK, null));

        _result = await _sut.Handle(_command, CancellationToken.None);
    }

    [Test]
    public void Handle_GetsRequestDetails()
    {
        _prApiClientMock.Verify(m => m.GetRequest(_command.RequestId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_GetsOrganisationDetailsFromTprApi()
    {
        _tprApiClientMock.Verify(t => t.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(It.Is<GetPensionsRegulatorOrganisationsRequest>(r => r.PayeRef == _permissionRequest.EmployerPAYE && r.Aorn == _permissionRequest.EmployerAORN)), Times.Once);
    }

    [Test]
    public void Handle_GetsExistingUserProfile()
    => _employerProfilesApiMock.Verify(e => e.GetWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<GetEmployerUserAccountRequest>()), Times.Once);

    [Test]
    public void Handle_UpdatesUserProfile()
        => _employerProfilesApiMock.Verify(e => e.PutWithResponseCode<EmployerProfileUsersApiResponse>(It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Once);

    [Test]
    public void Handle_CreatesEmployerAccount()
    {
        _accountsApiClientMock.Verify(a => a.PostWithResponseCode<CreateAccountRequestBody, PostCreateAccountResponse>(It.Is<PostCreateAccountRequest>(r => true), true), Times.Once);
    }

    [Test]
    public void Handle_AcceptsPermissionRequest()
    {
        _prApiClientMock.Verify(p => p.AcceptCreateAccountRequest(_command.RequestId, It.Is<AcceptCreateAccountRequestBody>(b => b.ActionedBy == _command.UserRef.ToString() && b.AccountDetails.Id == _createAccountResponse.AccountId && b.AccountLegalEntityDetails.Id == _createAccountResponse.AccountLegalEntityId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_SendsNotifications()
    {
        _prApiClientMock.Verify(p => p.PostNotifications(It.Is<PostNotificationsRequest>(r => r.Notifications.Length == 2), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_ReturnsAccountId()
    {
        _result.AccountId.Should().Be(_createAccountResponse.AccountId);
    }
}
