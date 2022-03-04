using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenGettingAccountLegalEntityByAccountIdentifier
    {
        [Test, MoqAutoData]
        public async Task Then_If_Employer_The_Accounts_Api_Is_Checked(
            string accountLegalEntityPublicHashedId,
            AccountDetail accountDetailApiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            legalEntities.First().AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{accountIdentifier.AccountHashedId}"))))
                .ReturnsAsync(accountDetailApiResponse);
            for (var i = 0; i < accountDetailApiResponse.LegalEntities.Count; i++)
            {
                var index = i;
                accountsApi
                    .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                        It.Is<GetEmployerAccountLegalEntityRequest>(request =>
                            request.GetUrl.Equals(accountDetailApiResponse.LegalEntities[index].Href))))
                    .ReturnsAsync(legalEntities[index]);
            }

            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);

            actual.Name.Should().Be(legalEntities.First().AccountLegalEntityName);
            actual.AccountLegalEntityPublicHashedId.Should().Be(legalEntities.First().AccountLegalEntityPublicHashedId);
            actual.AccountHashedId.Should().Be(accountIdentifier.AccountHashedId);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Employer_AccountLegalEntity_Does_Not_Exist_Then_Null_Returned(
            string accountLegalEntityPublicHashedId,
            AccountDetail accountDetailApiResponse,
            GetEmployerAccountLegalEntityItem legalEntityItem,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{accountIdentifier.AccountHashedId}"))))
                .ReturnsAsync(accountDetailApiResponse);
            accountsApi
                .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                    It.IsAny<GetEmployerAccountLegalEntityRequest>()))
                .ReturnsAsync(legalEntityItem);
            
            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);

            actual.Should().BeNull();
            accountsApi.Verify(x=>x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Account_Not_Found_Then_Null_Returned(
            string accountLegalEntityPublicHashedId,
            AccountDetail accountDetailApiResponse,
            GetEmployerAccountLegalEntityItem legalEntityItem,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{accountIdentifier.AccountHashedId}"))))
                .ReturnsAsync((AccountDetail)null);
            
            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);

            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Provider_Not_Found_Then_Null_Returned(
            string accountLegalEntityPublicHashedId,
            AccountDetail accountDetailApiResponse,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier("Provider-123456-Product");
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetProviderAccountLegalEntitiesRequest>(c =>
                    c.GetUrl.Contains(accountIdentifier.Ukprn.ToString())))).ReturnsAsync((GetProviderAccountLegalEntitiesResponse)null);
            
            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);

            actual.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Provider_The_Provider_Relations_Api_Is_Checked(
            int ukprn,
            string accountLegalEntityPublicHashedId,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier($"Provider-{ukprn}-Product");
            response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetProviderAccountLegalEntitiesRequest>(c =>
                    c.GetUrl.Contains(accountIdentifier.Ukprn.ToString())))).ReturnsAsync(response);

            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);
            
            actual.Name.Should().Be(response.AccountProviderLegalEntities.First().AccountLegalEntityName);
            actual.AccountLegalEntityPublicHashedId.Should().Be(response.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId);
            actual.AccountHashedId.Should().Be(response.AccountProviderLegalEntities.First().AccountHashedId);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Provider_And_AccountLegalEntity_Does_Not_Exist_Then_Null_Returned(
            int ukprn,
            string accountLegalEntityPublicHashedId,
            GetProviderAccountLegalEntitiesResponse response,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            AccountLegalEntityPermissionService service)
        {
            var accountIdentifier = new AccountIdentifier($"Provider-{ukprn}-Product");
            providerRelationshipsApiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesResponse>(It.Is<GetProviderAccountLegalEntitiesRequest>(c =>
                    c.GetUrl.Contains(accountIdentifier.Ukprn.ToString())))).ReturnsAsync(response);

            var actual = await service.GetAccountLegalEntity(accountIdentifier, accountLegalEntityPublicHashedId);
            
            actual.Should().BeNull();
            providerRelationshipsApiClient.Verify(x=>x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Once);
        }
    }
}