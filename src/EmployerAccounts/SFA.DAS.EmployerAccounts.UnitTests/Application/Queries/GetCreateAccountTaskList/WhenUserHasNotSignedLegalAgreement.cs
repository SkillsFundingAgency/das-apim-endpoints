using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenUserHasNotSignedLegalAgreement
{
    [Test, MoqAutoData]
    public async Task Then_GetAccountAgreements(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetCreateAccountTaskListQueryHandler sut,
        GetCreateAccountTaskListQuery query
    )
    {
        await sut.Handle(query, CancellationToken.None);

        accountsApiClient
            .Verify(x => 
                    x.GetAll<GetEmployerAgreementsResponse>(
                    It.Is<GetEmployerAgreementsRequest>(r => r.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements")))
                , Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Then_AccountHasPayeSchemes_Should_Return_HasPaye_True(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetAccountProvidersResponse accountProvidersResponse,
        GetCreateAccountTaskListQueryHandler sut,
        GetCreateAccountTaskListQuery query,
        GetAccountByIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        long agreementId
    )
    {
        accountResponse.NameConfirmed.Should().BeTrue();

        employerAgreementsResponse.Clear();
        employerAgreementsResponse.Add(new GetEmployerAgreementsResponse
        {
            Id = agreementId,
            AccountId = query.AccountId,
            Acknowledged = false
        });

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);
        
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

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.NameConfirmed.Should().BeTrue();
        actual.CompletedSections.Should().Be(3);
    }
}