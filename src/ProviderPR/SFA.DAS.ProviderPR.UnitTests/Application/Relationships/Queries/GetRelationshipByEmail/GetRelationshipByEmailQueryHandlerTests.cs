﻿using AutoFixture.NUnit3;
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
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Relationships.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_GetsNotFoundFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        string email,
        long ukprn,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse(), HttpStatusCode.NotFound, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(response);

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult();

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
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse(), HttpStatusCode.InternalServerError, "");

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
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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

        var expectedResponse = new GetRelationshipByEmailQueryResult();

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
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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

        var expectedResponse = new GetRelationshipByEmailQueryResult { HasUserAccount = true, HasOneEmployerAccount = false };

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
        GetUserAccountsResponse getUserAccountsResponse,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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
                getUserAccountsResponse
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{getUserAccountsResponse.EncodedAccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                new(),
                new()
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult
        {
            HasUserAccount = true,
            HasOneEmployerAccount = true,
            AccountId = getUserAccountsResponse.AccountId,
            HasOneLegalEntity = false
        };

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
        GetUserAccountsResponse getUserAccountsResponse,
        GetAccountLegalEntityResponse accountLegalEntityResponse,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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
                getUserAccountsResponse
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{getUserAccountsResponse.AccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                accountLegalEntityResponse
            });

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult
        {
            HasUserAccount = true,
            HasOneEmployerAccount = true,
            AccountId = getUserAccountsResponse.AccountId,
            HasOneLegalEntity = true,
            AccountLegalEntityPublicHashedId = accountLegalEntityResponse.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = accountLegalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = accountLegalEntityResponse.Name,
            HasRelationship = false
        };

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Once);
    }


    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_InvalidResponseFromProviderApi_ReturnsExpectedResult(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetAccountLegalEntityResponse getAccountLegalEntityResponse,
        GetUserAccountsResponse getUserAccountsResponse,
        GetRelationshipResponse expected,
        CancellationToken cancellationToken
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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
                getUserAccountsResponse
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{getUserAccountsResponse.AccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                getAccountLegalEntityResponse
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

        var expectedResponse = new GetRelationshipByEmailQueryResult
        {
            HasUserAccount = true,
            HasOneEmployerAccount = true,
            AccountId = getUserAccountsResponse.AccountId,
            HasOneLegalEntity = true,
            AccountLegalEntityPublicHashedId = getAccountLegalEntityResponse.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = getAccountLegalEntityResponse.Name,
            HasRelationship = false
        };

        actual.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_ValidResponseFromProviderApi_ReturnsExpectedResult(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
       [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
       string email,
       long ukprn,
       Guid userRef,
       GetAccountLegalEntityResponse getAccountLegalEntityResponse,
       GetUserAccountsResponse getUserAccountsResponse,
       GetRelationshipResponse expected,
       CancellationToken cancellationToken
   )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

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
                getUserAccountsResponse
            });

        accountsApiClient
            .Setup(x => x.GetAll<GetAccountLegalEntityResponse>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains($"api/accounts/{getUserAccountsResponse.AccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                getAccountLegalEntityResponse
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

        var expectedResponse = new GetRelationshipByEmailQueryResult
        {
            HasUserAccount = true,
            HasOneEmployerAccount = true,
            AccountId = getUserAccountsResponse.AccountId,
            HasOneLegalEntity = true,
            AccountLegalEntityPublicHashedId = getAccountLegalEntityResponse.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = getAccountLegalEntityResponse.Name,
            HasRelationship = true,
            Operations = expected.Operations.ToList()
        };

        actual.Should().BeEquivalentTo(expectedResponse);
    }
}
