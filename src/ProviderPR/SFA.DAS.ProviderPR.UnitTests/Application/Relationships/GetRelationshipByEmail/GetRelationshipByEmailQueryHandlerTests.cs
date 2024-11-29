using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipByEmail;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Queries.GetRelationshipByEmail;
public class GetRelationshipByEmailQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_GetsActiveRequestFromApi_ReturnsHasActiveRequestTrue(
      [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
      [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
      string email,
      long ukprn,
      GetRequestByUkprnAndEmailResponse requestResponse,
      GetRelationshipByEmailQueryHandler handler,
      CancellationToken cancellationToken
  )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndEmail(
                ukprn,
                email,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndEmailResponse>(
                string.Empty,
                new(HttpStatusCode.OK),
                () => requestResponse
            )
        );

        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipByEmailQueryResult { HasActiveRequest = true };

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Never);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Never);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
        providerRelationshipsApiRestClient.Verify(r => r.GetRequestByUkprnAndEmail(ukprn, email, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsActiveRequestFromApi_ReturnsUnexpectedStatus_ThrowException(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRelationshipByEmailQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndEmail(
                ukprn,
                email,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndEmailResponse>(
                string.Empty,
                new(HttpStatusCode.BadRequest),
                () => requestResponse
            )
        );

        Func<Task> act = async () => await handler.Handle(request, cancellationToken);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Provider PR API threw unexpected response for ukprn {ukprn} and email {email}");
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNotFoundFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        long accountLegalEntityId,
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
        GetRelationshipByEmailQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse(), HttpStatusCode.NotFound, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(response);

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, cancellationToken);
        SetupPrRestApiToReturnExistingRequestNotFound(providerRelationshipsApiRestClient, accountLegalEntityId, ukprn, requestUkprnAccountLegalEntityIdResponse, cancellationToken);
        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipByEmailQueryResult { HasUserAccount = false };

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Never);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
        providerRelationshipsApiRestClient.Verify(r => r.GetRequestByUkprnAndEmail(ukprn, email, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNotOKFromApi_ThrowsException(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRelationshipByEmailQueryHandler handler
    )
    {
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response = new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse(), HttpStatusCode.InternalServerError, "");

        accountsApiClient
            .Setup(x => x.GetWithResponseCode<GetUserByEmailResponse>(
                It.Is<GetUserByEmailRequest>(c => c.GetUrl.Contains($"api/user?email={email}"))))!
            .ReturnsAsync(response);

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, new CancellationToken());

        Func<Task> act = async () => await handler.Handle(request, new CancellationToken()); ;

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Error calling get user by email for {request.Email}");
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsNoAccountsFromApi_ReturnsHasUserAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetRequestByUkprnAndEmailResponse requestResponse,
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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, new CancellationToken());

        var actual = await handler.Handle(request, new CancellationToken());

        var expectedResponse = new GetRelationshipByEmailQueryResult { HasUserAccount = false };

        actual.Should().BeEquivalentTo(expectedResponse);
        accountsApiClient.Verify(r => r.GetWithResponseCode<GetUserByEmailResponse>(It.IsAny<GetUserByEmailRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetUserAccountsResponse>(It.IsAny<GetUserAccountsRequest>()), Times.Once);
        accountsApiClient.Verify(r => r.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsMoreThanOneAccount_ReturnsHaOneEmployerAccountFalse(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetRequestByUkprnAndEmailResponse requestResponse,
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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, new CancellationToken());

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
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetUserAccountsResponse getUserAccountsResponse,
        GetRequestByUkprnAndEmailResponse requestResponse,
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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, new CancellationToken());

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
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
        GetUserAccountsResponse getUserAccountsResponse,
        GetAccountLegalEntityResponse accountLegalEntityResponse,
        GetRelationshipByEmailQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var accountLegalEntityId = accountLegalEntityResponse.AccountLegalEntityId;
        requestResponse.AccountLegalEntityId = accountLegalEntityId;
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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, cancellationToken);
        SetupPrRestApiToReturnExistingRequestNotFound(providerRelationshipsApiRestClient, accountLegalEntityId, ukprn, requestUkprnAccountLegalEntityIdResponse, cancellationToken);

        var actual = await handler.Handle(request, cancellationToken);

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
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
        GetAccountLegalEntityResponse getAccountLegalEntityResponse,
        GetUserAccountsResponse getUserAccountsResponse,
        GetRelationshipResponse expected,
        CancellationToken cancellationToken
    )
    {
        var accountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId;

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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, cancellationToken);
        SetupPrRestApiToReturnExistingRequestNotFound(providerRelationshipsApiRestClient, accountLegalEntityId, ukprn, requestUkprnAccountLegalEntityIdResponse, cancellationToken);

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
            HasActiveRequest = false,
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
    public async Task Handle_GetsOneLegalEntity_HasExistingRequest_ReturnsExpectedResult(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        string email,
        long ukprn,
        Guid userRef,
        GetRequestByUkprnAndEmailResponse requestResponse,
        GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
        GetAccountLegalEntityResponse getAccountLegalEntityResponse,
        GetUserAccountsResponse getUserAccountsResponse,
        CancellationToken cancellationToken
    )
    {
        var accountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId;
        requestResponse.AccountLegalEntityId = accountLegalEntityId;
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response =
            new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK,
                "");

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
                It.Is<GetAccountLegalEntitiesRequest>(c =>
                    c.GetAllUrl.Contains(
                        $"api/accounts/{getUserAccountsResponse.AccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                getAccountLegalEntityResponse
            });

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse,
            cancellationToken);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndAccountLegalEntityId(
                ukprn,
                accountLegalEntityId,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndAccountLegalEntityIdResponse>(
                string.Empty,
                new(HttpStatusCode.OK),
                () => requestUkprnAccountLegalEntityIdResponse
            )
        );

        GetRelationshipByEmailQueryHandler handler = new GetRelationshipByEmailQueryHandler(accountsApiClient.Object, providerRelationshipsApiRestClient.Object);

        var actual = await handler.Handle(request, cancellationToken);

        var expectedResponse = new GetRelationshipByEmailQueryResult
        {
            HasActiveRequest = true,
            HasUserAccount = true,
            HasOneEmployerAccount = true,
            AccountId = getUserAccountsResponse.AccountId,
            HasOneLegalEntity = true,
            AccountLegalEntityPublicHashedId = getAccountLegalEntityResponse.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId,
            AccountLegalEntityName = getAccountLegalEntityResponse.Name,
            HasRelationship = null,
            Operations = []
        };

        actual.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsActiveExistingRequestFromApi_ReturnsUnexpectedStatus_ThrowException(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
       [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
       string email,
       long ukprn,
       Guid userRef,
       GetRequestByUkprnAndEmailResponse requestResponse,
       GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
       GetAccountLegalEntityResponse getAccountLegalEntityResponse,
       GetUserAccountsResponse getUserAccountsResponse,
       CancellationToken cancellationToken
   )
    {
        var accountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId;
        requestResponse.AccountLegalEntityId = accountLegalEntityId;
        var request = new GetRelationshipByEmailQuery(email, ukprn);

        var response =
            new ApiResponse<GetUserByEmailResponse>(new GetUserByEmailResponse { Ref = userRef }, HttpStatusCode.OK,
                "");

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
                It.Is<GetAccountLegalEntitiesRequest>(c =>
                    c.GetAllUrl.Contains(
                        $"api/accounts/{getUserAccountsResponse.AccountId}/legalentities?includeDetails=true"))))!
            .ReturnsAsync(new List<GetAccountLegalEntityResponse>
            {
                getAccountLegalEntityResponse
            });

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse,
            cancellationToken);

        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndAccountLegalEntityId(
                ukprn,
                accountLegalEntityId,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndAccountLegalEntityIdResponse>(
                string.Empty,
                new(HttpStatusCode.BadRequest),
                () => requestUkprnAccountLegalEntityIdResponse
            )
        );

        GetRelationshipByEmailQueryHandler handler = new GetRelationshipByEmailQueryHandler(accountsApiClient.Object, providerRelationshipsApiRestClient.Object);

        Func<Task> act = async () => await handler.Handle(request, cancellationToken);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Provider PR API threw unexpected response for ukprn {ukprn} and AccountLegalEntityId {accountLegalEntityId}");
    }

    [Test, MoqAutoData]
    public async Task Handle_GetsOneLegalEntity_ValidResponseFromProviderApi_ReturnsExpectedResult(
       [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
       [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
       string email,
       long ukprn,
       Guid userRef,
       GetRequestByUkprnAndEmailResponse requestResponse,
       GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse,
       GetAccountLegalEntityResponse getAccountLegalEntityResponse,
       GetUserAccountsResponse getUserAccountsResponse,
       GetRelationshipResponse expected,
       CancellationToken cancellationToken
   )
    {
        var accountLegalEntityId = getAccountLegalEntityResponse.AccountLegalEntityId;
        requestResponse.AccountLegalEntityId = accountLegalEntityId;
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

        SetupPrRestApiToReturnRequestNotFound(providerRelationshipsApiRestClient, email, ukprn, requestResponse, cancellationToken);
        SetupPrRestApiToReturnExistingRequestNotFound(providerRelationshipsApiRestClient, accountLegalEntityId, ukprn, requestUkprnAccountLegalEntityIdResponse, cancellationToken);

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
            HasActiveRequest = false,
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

    private static void SetupPrRestApiToReturnRequestNotFound(Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient, string email,
        long ukprn, GetRequestByUkprnAndEmailResponse requestResponse, CancellationToken cancellationToken)
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndEmail(
                ukprn,
                email,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndEmailResponse>(
                string.Empty,
                new(HttpStatusCode.NotFound),
                () => requestResponse
            )
        );
    }

    private static void SetupPrRestApiToReturnExistingRequestNotFound(Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient, long accountLegalEntityId,
        long ukprn, GetRequestByUkprnAndAccountLegalEntityIdResponse requestUkprnAccountLegalEntityIdResponse, CancellationToken cancellationToken)
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRequestByUkprnAndAccountLegalEntityId(
                ukprn,
                accountLegalEntityId,
                cancellationToken
            )
        )!.ReturnsAsync(
            new Response<GetRequestByUkprnAndAccountLegalEntityIdResponse>(
                string.Empty,
                new(HttpStatusCode.NotFound),
                () => requestUkprnAccountLegalEntityIdResponse
            )
        );
    }
}
