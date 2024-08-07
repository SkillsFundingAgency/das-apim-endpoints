using System.Collections.Generic;
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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenUserHasNotAcknowledgedTrainingProvider
{
    [Test, MoqAutoData]
    public async Task Then_AgreementAcknowledged_Should_Return_NameConfirmed_True(
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
        accountResponse.NameConfirmed = true;
        
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
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse);

        var actual = await sut.Handle(query, CancellationToken.None);
        
        actual.Should().NotBeNull();

        actual.AgreementAcknowledged.Should().BeTrue();
        actual.CompletedSections.Should().Be(4);
    }
}