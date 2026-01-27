using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Rofjaa;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Rofjaa;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class GetProviderRelationshipServiceTests
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Call_Api_WithcorrectValues(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationships,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsClient,
        [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
        List<GetProviderAccountLegalEntityItem> providerLegalEnities,
        ApprenticeshipEmployerType employerType,
        GetProviderRelationshipService sut)
    {
        var providerResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = providerLegalEnities
        };

        foreach (var provider in providerLegalEnities)
        {
            accountsClient.Setup(x => x.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(provider.AccountId))).
                ReturnsAsync(new GetAccountByIdResponse()
                {
                    ApprenticeshipEmployerType = ApprenticeshipEmployerType.Levy,
                });

            fjaaApiClient.Setup(x => x.Get<GetAgencyResponse>(new GetAgencyQuery(provider.AccountLegalEntityId))).
                ReturnsAsync(new GetAgencyResponse()
                {
                    IsGrantFunded = true
                });
        }

        // Act
        var details = await sut.GetEmployerDetails(providerResponse);

        //Assert
        details.Should().NotBeNull();
        details.Should().HaveCount(providerResponse.AccountProviderLegalEntities.Count);

        foreach (var p in providerResponse.AccountProviderLegalEntities)
        {
            accountsClient.Verify(x => x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(t => t.AccountId == p.AccountId)), Times.Once());

            fjaaApiClient.Verify(x => x.Get<GetAgencyResponse>(It.Is<GetAgencyQuery>(t => t.LegalEntityId == p.AccountLegalEntityId)), Times.Once());
        }
    }
}