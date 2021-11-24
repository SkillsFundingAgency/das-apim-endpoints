﻿using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using AccountDetail = SFA.DAS.Vacancies.InnerApi.Responses.AccountDetail;
using GetProviderAccountLegalEntitiesResponse = SFA.DAS.Vacancies.InnerApi.Responses.GetProviderAccountLegalEntitiesResponse;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancies_Returned(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);

        }

        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_Is_Null_Then_No_LegalEntity_Check_Is_Performed(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse, 
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);

            await handler.Handle(query, CancellationToken.None);
            
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            providerRelationshipsApiClient.Verify(x=>x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_ProviderRelations_Api_Checked(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse, 
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.AccountPublicHashedId = "";
            providerAccountLegalEntitiesResponse.AccountProviderLegalEntities.First().AccountLegalEntityPublicHashedId =
                query.AccountLegalEntityPublicHashedId;
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            providerRelationshipsApiClient
                .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                    It.Is<GetProviderAccountLegalEntitiesRequest>(c => c.GetUrl.Contains($"ukprn={query.Ukprn}&"))))
                .ReturnsAsync(providerAccountLegalEntitiesResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public void And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_ProviderRelations_Api_Checked_And_If_Not_In_Response_Exception_Thrown(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse, 
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.AccountPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            providerRelationshipsApiClient
                .Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(
                    It.Is<GetProviderAccountLegalEntitiesRequest>(c => c.GetUrl.Contains($"ukprn={query.Ukprn}&"))))
                .ReturnsAsync(providerAccountLegalEntitiesResponse);

            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(query, CancellationToken.None));
            
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public void And_The_AccountIds_And_Ukprn_Is_Not_Null_Then_Accounts_Api_Checked_And_If_Not_In_Response_Exception_Thrown(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse, 
            AccountDetail accountDetailApiResponse,
            GetEmployerAccountLegalEntityItem legalEntity,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{query.AccountPublicHashedId}"))))
                .ReturnsAsync(accountDetailApiResponse);
            accountsApi
                .Setup(client => client.Get<GetEmployerAccountLegalEntityItem>(
                    It.IsAny<GetEmployerAccountLegalEntityRequest>()))
                .ReturnsAsync(legalEntity);

            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(query, CancellationToken.None));
            
            providerRelationshipsApiClient.Verify(x=>x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
        }
        
        [Test, MoqAutoData]
        public async Task And_The_AccountIds_And_Ukprn_Is_Not_Null_Then_Accounts_Api_Checked_And_Then_Accounts_Api_Checked(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse, 
            AccountDetail accountDetailApiResponse,
            List<GetEmployerAccountLegalEntityItem> legalEntities,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            legalEntities.First().AccountLegalEntityPublicHashedId =
                query.AccountLegalEntityPublicHashedId;
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            accountsApi
                .Setup(x => x.Get<AccountDetail>(
                    It.Is<GetAllEmployerAccountLegalEntitiesRequest>(c => c.GetUrl.EndsWith($"accounts/{query.AccountPublicHashedId}"))))
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

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
            providerRelationshipsApiClient.Verify(x=>x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_AccountLegalEntityPublicHashedId_And_No_Account_Or_Ukprn_Then_Exception_Thrown(
                GetVacanciesQuery query,
                GetVacanciesResponse apiResponse, 
                AccountDetail accountDetailApiResponse,
                [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
                [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
                [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
                GetVacanciesQueryHandler handler)
        {
            query.AccountPublicHashedId = null;
            query.Ukprn = null;

            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(query, CancellationToken.None));
            
            providerRelationshipsApiClient.Verify(x=>x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            apiClient.Verify(x => x.Get<GetVacanciesResponse>(It.IsAny<GetVacanciesRequest>()), Times.Never);
        }
    }
}
