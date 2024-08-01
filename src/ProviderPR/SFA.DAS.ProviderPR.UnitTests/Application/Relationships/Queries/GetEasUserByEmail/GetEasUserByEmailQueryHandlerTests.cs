using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Relationships.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_GetsNotFoundFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>((GetUserByEmailResponse)null, HttpStatusCode.NotFound, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(response);

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetEasUserByEmailQueryResult(false, null, null, null);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Never);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNotOKFromApi_ThrowsException(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>((GetUserByEmailResponse)null, HttpStatusCode.InternalServerError, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(response);

        Func<Task> act = async () => await handler.Handle(request, new CancellationToken()); ;

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Error calling get user by email for {request.Email}");
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNoAccountsFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        Guid userRef,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.IsAny<GetUserByEmailRequest>()))!
            .ReturnsAsync(response);

        accountsApiClient
            .Setup(x => x.GetAll<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"api/user/{userRef}/accounts"))))!
            .ReturnsAsync(new List<GetUserAccountsResponse>());

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetEasUserByEmailQueryResult(false, null, null, null);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
    }


    [Test, MoqAutoData]
    public async Task Handle_GetsMoreThanOneAccount_ReturnsHaOneEmployerAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        Guid userRef,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.IsAny<GetUserByEmailRequest>()))!
            .ReturnsAsync(response);

        accountsApiClient
            .Setup(x => x.GetAll<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"api/user/{userRef}/accounts"))))!
            .ReturnsAsync(new List<GetUserAccountsResponse>
            {
                new(),
                new()
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetEasUserByEmailQueryResult(true, false, null, null);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsMoreThanOneLegalEntity_ReturnsHasOneLegalEntityFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        Guid userRef,
        string encodedAccountId,
        long accountId,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.IsAny<GetUserByEmailRequest>()))!
            .ReturnsAsync(response);

        accountsApiClient
            .Setup(x => x.GetAll<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"api/user/{userRef}/accounts"))))!
            .ReturnsAsync(new List<GetUserAccountsResponse>
            {
                new() {EncodedAccountId = encodedAccountId, AccountId = accountId}
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{encodedAccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                new(),
                new()
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetEasUserByEmailQueryResult(true, true, accountId, false);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_ReturnsHasOneLegalEntityTrue(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        Guid userRef,
        string encodedAccountId,
        long accountId,
        GetEasUserByEmailQueryHandler handler
    )
    {
        var request = new GetEasUserByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.IsAny<GetUserByEmailRequest>()))!
            .ReturnsAsync(response);

        accountsApiClient
            .Setup(x => x.GetAll<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"api/user/{userRef}/accounts"))))!
            .ReturnsAsync(new List<GetUserAccountsResponse>
            {
                new() {EncodedAccountId = encodedAccountId, AccountId = accountId}
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{accountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                new()
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetEasUserByEmailQueryResult(true, true, accountId, true);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
    }
}
