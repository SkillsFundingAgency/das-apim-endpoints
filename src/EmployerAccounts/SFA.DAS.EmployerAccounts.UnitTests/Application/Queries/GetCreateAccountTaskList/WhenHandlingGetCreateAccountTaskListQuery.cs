using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerRegistration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenHandlingGetCreateAccountTaskListQuery
{
    [Test, MoqAutoData]
    public async Task When_There_Are_No_Agreements_Then_Null_Response_Is_Returned(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        GetAccountProvidersResponse accountProvidersResponse,
        GetAccountByIdResponse accountResponse,
        GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse);

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(providerRelationshipResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c => c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c => c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync([]);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().BeNull();

        providerRelationshipsApiClient
            .Verify(x =>
                    x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                        c.GetUrl.Equals($"accounts/{query.AccountId}/providers")))
                , Times.Once);

        accountsApiClient
            .Verify(x =>
                    x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                        c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                        c.GetUrl.Equals($"api/accounts/{query.AccountId}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                        c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements")))
                , Times.Once());
    }

    [Test, MoqAutoData]
    public async Task When_The_Data_Is_Populated_A_Response_Is_Returned(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetAccountProvidersResponse accountProvidersResponse,
        GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
        GetAccountByIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountResponse.AddTrainingProviderAcknowledged = true;

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse)
            .Verifiable();

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(providerRelationshipResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse)
            .Verifiable();

        var actual = await sut.Handle(query, CancellationToken.None);

        var firstAgreement = employerAgreementsResponse.FirstOrDefault();

        actual.Should().NotBeNull();
        actual.HashedAccountId.Should().Be(query.HashedAccountId);
        actual.HasPayeScheme.Should().Be(payeSchemesResponse.Count != 0);
        actual.NameConfirmed.Should().Be(accountResponse.NameConfirmed);
        actual.PendingAgreementId.Should().NotBeNull();
        actual.AddTrainingProviderAcknowledged.Should().Be(accountResponse.AddTrainingProviderAcknowledged);
        actual.UserFirstName.Should().Be(userResponse.FirstName);
        actual.UserLastName.Should().Be(userResponse.LastName);
        actual.AgreementAcknowledged.Should().Be(firstAgreement.Acknowledged ?? true);
        actual.HasSignedAgreement.Should().Be(firstAgreement.SignedDate.HasValue);
        actual.HasProviders.Should().BeTrue();
        actual.HasProviderPermissions.Should().Be(providerRelationshipResponse.AccountProviderLegalEntities.Any());

        providerRelationshipsApiClient.Verify();
        accountsApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task When_AcknowledgeTrainingProviderTask_Is_Outstanding_Then_It_Is_Updated_And_Returns_True_For_Property(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetAccountProvidersResponse accountProvidersResponse,
        GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
        GetAccountByIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountResponse.AddTrainingProviderAcknowledged = false;

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(providerRelationshipResponse);

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.HashedAccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.AddTrainingProviderAcknowledged.Should().BeTrue();

        accountsApiClient.Verify(x =>
                x.Patch(It.Is<AcknowledgeTrainingProviderTaskRequest>(
                    c =>
                        c.PatchUrl.Equals("api/accounts/acknowledge-training-provider-task")
                        && c.Data.Equals(new AcknowledgeTrainingProviderTaskData(query.AccountId))
                ))
            , Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_AcknowledgeTrainingProviderTask_Is_Not_Outstanding_Then_It_Is_Not_Updated(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
        GetAccountByIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetAccountProvidersResponse accountProvidersResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountResponse.AddTrainingProviderAcknowledged = true;

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetAccountProvidersResponse>(It.Is<GetAccountProvidersRequest>(c =>
                    c.GetUrl.Equals($"accounts/{query.AccountId}/providers"))))
            .ReturnsAsync(accountProvidersResponse);

        providerRelationshipsApiClient
            .Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetEmployerAccountProviderPermissionsRequest>(c =>
                    c.GetUrl.Equals($"accountproviderlegalentities?accountHashedId={query.HashedAccountId}&operations=1&operations=2"))))
            .ReturnsAsync(providerRelationshipResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.HashedAccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();

        accountsApiClient.Verify(x =>
                x.Patch(It.Is<AcknowledgeTrainingProviderTaskRequest>(
                    c =>
                        c.PatchUrl.Equals("api/acknowledge-training-provider-task")
                        && c.Data.Equals(new AcknowledgeTrainingProviderTaskData(query.AccountId))
                ))
            , Times.Never);
    }
}