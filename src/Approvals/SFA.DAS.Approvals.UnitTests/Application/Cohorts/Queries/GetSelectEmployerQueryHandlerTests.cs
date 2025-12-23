using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Queries;

[TestFixture]
public class GetSelectEmployerQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_ProviderRelationships_Api_Is_Called_And_Data_Returned(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };
        
        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1",
                    AccountPublicHashedId = "PUB1",
                    AccountLegalEntityId = 1,
                    AccountProviderId = 1
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Account 2",
                    AccountLegalEntityName = "Legal Entity 2",
                    AccountLegalEntityPublicHashedId = "PUB2",
                    AccountPublicHashedId = "PUB2",
                    AccountLegalEntityId = 2,
                    AccountProviderId = 2
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                It.Is<GetProviderAccountLegalEntitiesRequest>(r => 
                    r.GetUrl.Contains(query.ProviderId.ToString()))))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "NonLevy" });

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(2);
        actual.Employers.Should().NotBeNull();
        actual.Employers.Should().Contain("Legal Entity 1");
        actual.Employers.Should().Contain("Legal Entity 2");
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_ProviderRelationships_Response_Then_Empty_Result_Returned(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };
        
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync((GetProviderAccountLegalEntitiesResponse)null);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().BeEmpty();
        actual.Employers.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Empty_ProviderRelationships_List_Then_Empty_Result_Returned(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };
        
        var emptyResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = []
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(emptyResponse);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().BeEmpty();
        actual.Employers.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_Levy_Status_Is_Enriched_From_Accounts_Api(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };
        
        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Account 2",
                    AccountLegalEntityName = "Legal Entity 2",
                    AccountLegalEntityPublicHashedId = "PUB2"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "NonLevy" });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().HaveCount(2);
        actual.AccountProviderLegalEntities[0].ApprenticeshipEmployerType.Should().Be("Levy");
        actual.AccountProviderLegalEntities[1].ApprenticeshipEmployerType.Should().Be("NonLevy");
    }

    [Test, MoqAutoData]
    public async Task Then_Default_To_NonLevy_When_Account_Not_Found(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };
        
        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync((GetAccountResponse)null);

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().HaveCount(1);
        actual.AccountProviderLegalEntities.First().ApprenticeshipEmployerType.Should().Be("NonLevy");
    }

    [Test, MoqAutoData]
    public async Task Then_Search_Filter_Is_Applied_When_SearchTerm_Provided(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery 
        { 
            ProviderId = 100005067,
            SearchTerm = "Test"
        };

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Test Account",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Other Account",
                    AccountLegalEntityName = "Legal Entity 2",
                    AccountLegalEntityPublicHashedId = "PUB2"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().HaveCount(1);
        actual.AccountProviderLegalEntities[0].AccountName.Should().Be("Test Account");
    }

    [Test, MoqAutoData]
    public async Task Then_Sort_Is_Applied_When_SortField_Provided(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery 
        { 
            ProviderId = 100005067,
            SortField = "EmployerAccountLegalEntityName",
            ReverseSort = false
        };

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "C Legal Entity",
                    AccountLegalEntityPublicHashedId = "PUB1"
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Account 2",
                    AccountLegalEntityName = "A Legal Entity",
                    AccountLegalEntityPublicHashedId = "PUB2"
                },
                new()
                {
                    AccountId = 3,
                    AccountHashedId = "HASH3",
                    AccountName = "Account 3",
                    AccountLegalEntityName = "B Legal Entity",
                    AccountLegalEntityPublicHashedId = "PUB3"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH3")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().HaveCount(3);
        actual.AccountProviderLegalEntities[0].AccountLegalEntityName.Should().Be("A Legal Entity");
        actual.AccountProviderLegalEntities[1].AccountLegalEntityName.Should().Be("B Legal Entity");
        actual.AccountProviderLegalEntities[2].AccountLegalEntityName.Should().Be("C Legal Entity");
    }

    [Test, MoqAutoData]
    public async Task Then_Employers_List_Is_Generated_For_Autocomplete(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Account 2",
                    AccountLegalEntityName = "Legal Entity 2",
                    AccountLegalEntityPublicHashedId = "PUB2"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.Employers.Should().Contain("Legal Entity 1");
        actual.Employers.Should().Contain("Legal Entity 2");
        actual.Employers.Should().Contain("Account 1");
        actual.Employers.Should().Contain("Account 2");
    }

    [Test, MoqAutoData]
    public async Task Then_Handles_Account_Api_Errors_Gracefully(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        [Greedy] GetSelectEmployerQueryHandler handler)
    {
        // Arrange
        var query = new GetSelectEmployerQuery { ProviderId = 100005067 };

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities =
            [
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                },
                new()
                {
                    AccountId = 2,
                    AccountHashedId = "HASH2",
                    AccountName = "Account 2",
                    AccountLegalEntityName = "Legal Entity 2",
                    AccountLegalEntityPublicHashedId = "PUB2"
                }
            ]
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ThrowsAsync(new System.Exception("API Error"));
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        // Act
        var actual = await handler.Handle(query, CancellationToken.None);

        // Assert
        actual.AccountProviderLegalEntities.Should().HaveCount(2);
        var hash1Entity = actual.AccountProviderLegalEntities.First(x => x.AccountHashedId == "HASH1");
        var hash2Entity = actual.AccountProviderLegalEntities.First(x => x.AccountHashedId == "HASH2");
        hash1Entity.ApprenticeshipEmployerType.Should().Be("NonLevy");
        hash2Entity.ApprenticeshipEmployerType.Should().Be("Levy");
    }
}
