using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Queries;

[TestFixture]
public class GetSelectEmployerQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_ProviderRelationships_Api_Is_Called_And_Data_Returned(
        GetSelectEmployerQuery query,
        GetProviderAccountLegalEntitiesResponse providerRelationshipsResponse,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        GetSelectEmployerQueryHandler handler)
    {
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                It.Is<GetProviderAccountLegalEntitiesRequest>(r => 
                    r.GetUrl.Contains(query.ProviderId.ToString()))))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountHashedIds = providerRelationshipsResponse.AccountProviderLegalEntities
            .Where(x => !string.IsNullOrWhiteSpace(x.AccountHashedId))
            .Select(x => x.AccountHashedId)
            .Distinct()
            .ToList();

        foreach (var accountHashedId in accountHashedIds)
        {
            accountsApiClient
                .Setup(x => x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(r => r.HashedAccountId == accountHashedId)))
                .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        }

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(providerRelationshipsResponse.AccountProviderLegalEntities.Count);
        actual.Employers.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_ProviderRelationships_Response_Then_Empty_Result_Returned(
        GetSelectEmployerQuery query,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        GetSelectEmployerQueryHandler handler)
    {
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync((GetProviderAccountLegalEntitiesResponse)null);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().BeEmpty();
        actual.Employers.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Empty_ProviderRelationships_List_Then_Empty_Result_Returned(
        GetSelectEmployerQuery query,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        [Frozen] Mock<ILogger<GetSelectEmployerQueryHandler>> logger,
        GetSelectEmployerQueryHandler handler)
    {
        var emptyResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>()
        };

        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(emptyResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().BeEmpty();
        actual.Employers.Should().BeEmpty();
    }

    [Test]
    public async Task Then_Levy_Status_Is_Enriched_From_Accounts_Api()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();
        
        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
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
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "NonLevy" });

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(2);
        actual.AccountProviderLegalEntities[0].ApprenticeshipEmployerType.Should().Be("Levy");
        actual.AccountProviderLegalEntities[1].ApprenticeshipEmployerType.Should().Be("NonLevy");
    }

    [Test]
    public async Task Then_Default_To_NonLevy_When_Account_Not_Found()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();
        
        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
                new()
                {
                    AccountId = 1,
                    AccountHashedId = "HASH1",
                    AccountName = "Account 1",
                    AccountLegalEntityName = "Legal Entity 1",
                    AccountLegalEntityPublicHashedId = "PUB1"
                }
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ReturnsAsync((GetAccountResponse)null);

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities[0].ApprenticeshipEmployerType.Should().Be("NonLevy");
    }

    [Test]
    public async Task Then_Search_Filter_Is_Applied_When_SearchTerm_Provided()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();
        query.SearchTerm = "Test";

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
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
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.IsAny<GetAccountRequest>()))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(1);
        actual.AccountProviderLegalEntities[0].AccountName.Should().Be("Test Account");
    }

    [Test]
    public async Task Then_Sort_Is_Applied_When_SortField_Provided()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();
        query.SortField = "EmployerAccountLegalEntityName";
        query.ReverseSort = false;

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
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
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.IsAny<GetAccountRequest>()))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(3);
        actual.AccountProviderLegalEntities[0].AccountLegalEntityName.Should().Be("A Legal Entity");
        actual.AccountProviderLegalEntities[1].AccountLegalEntityName.Should().Be("B Legal Entity");
        actual.AccountProviderLegalEntities[2].AccountLegalEntityName.Should().Be("C Legal Entity");
    }

    [Test]
    public async Task Then_Employers_List_Is_Generated_For_Autocomplete()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
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
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.IsAny<GetAccountRequest>()))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Employers.Should().Contain("Legal Entity 1");
        actual.Employers.Should().Contain("Legal Entity 2");
        actual.Employers.Should().Contain("Account 1");
        actual.Employers.Should().Contain("Account 2");
    }

    [Test]
    public async Task Then_Handles_Account_Api_Errors_Gracefully()
    {
        var fixture = new Fixture();
        var query = fixture.Create<GetSelectEmployerQuery>();

        var providerRelationshipsResponse = new GetProviderAccountLegalEntitiesResponse
        {
            AccountProviderLegalEntities = new List<GetProviderAccountLegalEntityItem>
            {
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
            }
        };

        var providerRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>>();
        providerRelationshipsApiClient
            .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
            .ReturnsAsync(providerRelationshipsResponse);

        var accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH1")))
            .ThrowsAsync(new System.Exception("API Error"));
        accountsApiClient
            .Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r => r.HashedAccountId == "HASH2")))
            .ReturnsAsync(new GetAccountResponse { ApprenticeshipEmployerType = "Levy" });

        var logger = new Mock<ILogger<GetSelectEmployerQueryHandler>>();
        var handler = new GetSelectEmployerQueryHandler(
            providerRelationshipsApiClient.Object,
            accountsApiClient.Object,
            logger.Object);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().HaveCount(2);
        actual.AccountProviderLegalEntities[0].ApprenticeshipEmployerType.Should().Be("NonLevy");
        actual.AccountProviderLegalEntities[1].ApprenticeshipEmployerType.Should().Be("Levy");
    }
}
