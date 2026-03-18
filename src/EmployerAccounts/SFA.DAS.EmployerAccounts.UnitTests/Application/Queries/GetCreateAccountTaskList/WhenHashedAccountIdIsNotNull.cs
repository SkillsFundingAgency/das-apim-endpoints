using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenHashedAccountIdIsNotNull
{
    [Test, MoqAutoData]
    public async Task And_Account_Is_Null_Then_Null_Response_Is_Returned(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetAccountProvidersResponse accountProvidersResponse,
        GetProviderAccountLegalEntitiesResponse providerRelationshipResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
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
            .ReturnsAsync(() => null)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse)
            .Verifiable();

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().BeNull();

        providerRelationshipsApiClient.Verify();
        accountsApiClient.Verify();
    }
}