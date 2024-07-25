using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Relationships.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_GetsNullFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providersApiRestClient,
        string email,
        long ukprn,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        accountsApiClient
            .Setup(x => x.Get<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync((GetUserByEmailResponse)null!);

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult(false, null, null, null);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.Get<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Never);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
        providersApiRestClient.Verify(
            p => p.GetRelationship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsMoreThanOneAccount_ReturnsHaOneEmployerAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providersApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        accountsApiClient
            .Setup(x => x.Get<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(new GetUserByEmailResponse { Ref = userRef });

        accountsApiClient
            .Setup(x => x.GetAll<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"api/user/{userRef}/accounts"))))!
            .ReturnsAsync(new List<GetUserAccountsResponse>
            {
                new(),
                new()
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult(true, false, null, null);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.Get<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
        providersApiRestClient.Verify(
            p => p.GetRelationship(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsMoreThanOneLegalEntity_ReturnsHasOneLegalEntityFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providersApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        string encodedAccountId,
        long accountId,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        accountsApiClient
            .Setup(x => x.Get<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(new GetUserByEmailResponse { Ref = userRef });

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

        var expectedResponse = new GetRelationshipByEmailQueryResult(true, true, accountId, false);

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.Get<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
        providersApiRestClient.Verify(
            p => p.GetRelationship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_ReturnsHasOneLegalEntityTrue(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        string encodedAccountId,
        long accountId,
        CancellationToken cancellationToken,
        GetRelationshipResponse expected,
        long accountLegalEntityId,
        string accountLegalEntityName,
        string accountPublicHashedId
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        accountsApiClient
            .Setup(x => x.Get<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(new GetUserByEmailResponse { Ref = userRef });

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
                new GetAccountLegalEntityResponse
                {
                    AccountLegalEntityId = accountLegalEntityId,
                    Name = accountLegalEntityName,
                    AccountLegalEntityPublicHashedId = accountPublicHashedId
                }
            });

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRelationship(
                It.IsAny<long>(),
                It.IsAny<long>(),
               cancellationToken
            )
        ).ReturnsAsync(
            new Response<GetRelationshipResponse>(
                string.Empty,
                new(HttpStatusCode.OK),
                () => expected
            )
        );


        GetRelationshipByEmailQueryHandler handler = new GetRelationshipByEmailQueryHandler(accountsApiClient.Object, providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipByEmailQueryResult(true, true, accountId, true, accountPublicHashedId, accountLegalEntityId, accountLegalEntityName, true, expected.Operations.ToList());

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.Get<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
        providerRelationshipsApiRestClient.Verify(
            p => p.GetRelationship(It.IsAny<long>(), It.IsAny<long>(), cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_InvalidResponseFromProviderApi_ReturnsExpectedResult(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
       [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
       string email,
       long ukprn,
       Guid userRef,
       string encodedAccountId,
       long accountId,
       CancellationToken cancellationToken,
       GetRelationshipResponse expected,
       long accountLegalEntityId,
       string accountLegalEntityName,
       string accountPublicHashedId
   )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        accountsApiClient
            .Setup(x => x.Get<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(new GetUserByEmailResponse { Ref = userRef });

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
                new GetAccountLegalEntityResponse
                {
                    AccountLegalEntityId = accountLegalEntityId,
                    Name = accountLegalEntityName,
                    AccountLegalEntityPublicHashedId = accountPublicHashedId
                }
            });

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRelationship(
                It.IsAny<long>(),
                It.IsAny<long>(),
               cancellationToken
            )
        ).ReturnsAsync(
            new Response<GetRelationshipResponse>(
                string.Empty,
                new(HttpStatusCode.NotFound),
                () => expected
            )
        );


        GetRelationshipByEmailQueryHandler handler = new GetRelationshipByEmailQueryHandler(accountsApiClient.Object, providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipByEmailQueryResult(true, true, accountId, true, accountPublicHashedId, accountLegalEntityId, accountLegalEntityName, false, new List<Operation>());

        actual.Should().BeEquivalentTo(expectedResponse);
    }
}
