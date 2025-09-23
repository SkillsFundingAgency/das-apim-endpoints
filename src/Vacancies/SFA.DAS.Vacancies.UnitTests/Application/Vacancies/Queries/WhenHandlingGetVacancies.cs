﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancies;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Enums;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancies_Returned(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            foreach (var apprenticeshipVacancy in apiResponse.ApprenticeshipVacancies)
            {
                apprenticeshipVacancy.VacancySource = DataSource.Raa;
            }
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
            actual.Total.Should().Be(apiResponse.Total);
            actual.TotalFiltered.Should().Be(apiResponse.TotalFound);
            actual.TotalPages.Should().Be((int)Math.Ceiling((decimal)apiResponse.TotalFound / query.PageSize));
            metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(apiResponse.ApprenticeshipVacancies.Count()));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancies_Returned_Non_Raa(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            foreach (var apprenticeshipVacancy in apiResponse.ApprenticeshipVacancies)
            {
                apprenticeshipVacancy.VacancySource = DataSource.Nhs;
            }
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
                { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
            actual.Total.Should().Be(apiResponse.Total);
            actual.TotalFiltered.Should().Be(apiResponse.TotalFound);
            actual.TotalPages.Should().Be((int)Math.Ceiling((decimal)apiResponse.TotalFound / query.PageSize));
            metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Results_From_Zero_Page_Size_Then_Response_Returned(
            GetVacanciesQuery query,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.PageSize = 0;
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(new GetVacanciesResponse
                    {
                        Total = 0,
                        ApprenticeshipVacancies = new List<GetVacanciesListItem>(),
                        TotalFound = 0
                    });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEmpty();
            actual.Total.Should().Be(0);
            actual.TotalFiltered.Should().Be(0);
            actual.TotalPages.Should().Be(0);
            metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Never());
        }
        [Test, MoqAutoData]
        public async Task Then_If_The_StandardLarsCode_Is_Null_Then_Not_Returned_In_Response(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacanciesQueryHandler handler)
        {
            foreach (var apprenticeshipVacancy in apiResponse.ApprenticeshipVacancies)
            {
                apprenticeshipVacancy.VacancySource = DataSource.Raa;
            }
            apiResponse.ApprenticeshipVacancies.First().StandardLarsCode = null;

            query.AccountLegalEntityPublicHashedId = "";

            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies.Where(c => c.StandardLarsCode != null).ToList());
            actual.Total.Should().Be(apiResponse.Total);
            actual.TotalFiltered.Should().Be(apiResponse.TotalFound);
            actual.TotalPages.Should().Be((int)Math.Ceiling((decimal)apiResponse.TotalFound / query.PageSize));
            metricsService.Verify(x => x.IncreaseVacancySearchResultViews(It.IsAny<string>(), 1), Times.Exactly(apiResponse.ApprenticeshipVacancies.Count(c => c.StandardLarsCode != null)));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Route_And_CourseTitle_Are_Taken_From_Standards_Service_And_Ignored_For_Null(
            int standardLarsCode,
            string findAnApprenticeshipBaseUrl,
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacanciesQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            courseResponse.LarsCode = standardLarsCode;
            foreach (var vacanciesItem in apiResponse.ApprenticeshipVacancies)
            {
                vacanciesItem.StandardLarsCode = standardLarsCode;
            }
            apiResponse.ApprenticeshipVacancies.First().StandardLarsCode = null;
            query.AccountLegalEntityPublicHashedId = "";
            query.Ukprn = null;
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.ToList().TrueForAll(c =>
                c.CourseTitle.Equals(courseResponse.Title)
                && c.Route.Equals(courseResponse.Route)
                && c.CourseLevel.Equals(courseResponse.Level)
                ).Should().BeTrue();
            foreach (var vacancy in actual.Vacancies)
            {
                vacancy.VacancyUrl.Should().Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancy.VacancyReference}");
            }
        }
        
        
        [Test, MoqAutoData]
        public async Task Then_The_VacancyUrl_Is_Correct_For_Each_Vacancy_Source(
            int standardLarsCode,
            string findAnApprenticeshipBaseUrl,
            GetVacanciesQuery query,
            GetVacanciesListItem vacancyRaa,
            GetVacanciesListItem vacancyNhs,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacanciesQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            courseResponse.LarsCode = standardLarsCode;
            vacancyRaa.VacancySource = DataSource.Raa;
            vacancyNhs.VacancySource = DataSource.Nhs;
            var apiResponse = new GetVacanciesResponse
            {
                ApprenticeshipVacancies = new List<GetVacanciesListItem> { vacancyRaa, vacancyNhs },
                TotalFound = 2,
                Total = 2
            };
            foreach (var vacanciesItem in apiResponse.ApprenticeshipVacancies)
            {
                vacanciesItem.StandardLarsCode = standardLarsCode;
            }
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Vacancies.First(x => x.Id.Equals(vacancyRaa.Id)).VacancyUrl.Should().Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancyRaa.VacancyReference}");
            actual.Vacancies.First(x => x.Id.Equals(vacancyNhs.Id)).VacancyUrl.Should().Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancyNhs.VacancyReference}");
        }

        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_Is_Null_Then_No_LegalEntity_Check_Is_Performed(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetStandardsListResponse courses,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            await handler.Handle(query, CancellationToken.None);

            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            providerRelationshipsApiClient.Verify(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_Permission_Checked(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            AccountLegalEntityItem accountLegalEntityItem,
            GetStandardsListResponse courses,
            [Frozen] Mock<IMetrics> metricsService,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes, query.PostedInLastNumberOfDays,
                query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(query.AccountIdentifier)),
                    query.AccountLegalEntityPublicHashedId)).ReturnsAsync(accountLegalEntityItem);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
        }

        [Test, MoqAutoData]
        public void And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_ProviderRelations_Api_Checked_And_If_Not_In_Response_Exception_Thrown(
            GetVacanciesQuery query,
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.AccountPublicHashedId = "";
            query.AccountIdentifier = new AccountIdentifier($"Employer-{query.Ukprn}-Product");
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(query.AccountIdentifier)),
                    query.AccountLegalEntityPublicHashedId)).ReturnsAsync((AccountLegalEntityItem)null);

            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(query, CancellationToken.None));

        }
        [Test, MoqAutoData]
        public async Task And_The_AccountIdentifier_Is_External_Then_HashedIds_Are_Not_Set(
            Guid externalId,
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            GetStandardsListResponse courses,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            query.Routes = new List<string>();
            query.AccountPublicHashedId = "";
            query.AccountIdentifier = new AccountIdentifier($"External-{externalId}-Product");
            var expectedGetRequest = new GetVacanciesRequest(query.PageNumber, query.PageSize,
                "", query.EmployerName, query.Ukprn, query.AccountPublicHashedId, query.StandardLarsCode,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, query.Routes,
                query.PostedInLastNumberOfDays, query.AdditionalDataSources, query.Sort, query.ExcludeNational);
            apiClient.Setup(x =>
                x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
            accountLegalEntityPermissionService
                .Verify(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(),
                    query.AccountLegalEntityPublicHashedId), Times.Never);
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

            providerRelationshipsApiClient.Verify(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            apiClient.Verify(x => x.Get<GetVacanciesResponse>(It.IsAny<GetVacanciesRequest>()), Times.Never);
        }
    }
}