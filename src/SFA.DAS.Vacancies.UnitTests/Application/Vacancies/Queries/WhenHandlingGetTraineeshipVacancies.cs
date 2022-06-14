﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetTraineeshipVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancies_Returned(
            GetTraineeshipVacanciesQuery query,
            GetTraineeshipVacanciesResponse apiResponse,
            List<string> categories,
            GetStandardsListItem courseResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(categories);
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, categories, query.PostedInLastNumberOfDays, query.Sort);
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.TraineeshipVacancies);
            actual.Total.Should().Be(apiResponse.Total);
            actual.TotalFiltered.Should().Be(apiResponse.TotalFound);
            actual.TotalPages.Should().Be((int)Math.Ceiling((decimal)apiResponse.TotalFound / query.PageSize));
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Results_From_Zero_Page_Size_Then_Response_Returned(
            GetTraineeshipVacanciesQuery query,
            List<string> categories,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.PageSize = 0;
            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(categories);
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, categories, query.PostedInLastNumberOfDays, query.Sort);
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(new GetTraineeshipVacanciesResponse
                    {
                        Total = 0,
                        TraineeshipVacancies = new List<GetTraineeshipVacanciesListItem>(),
                        TotalFound = 0
                    });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEmpty();
            actual.Total.Should().Be(0);
            actual.TotalFiltered.Should().Be(0);
            actual.TotalPages.Should().Be(0);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_RouteId_Is_Null_Then_Not_Returned_In_Response(
            GetTraineeshipVacanciesQuery query,
            GetTraineeshipVacanciesResponse apiResponse,
            GetStandardsListItem courseResponse,
            List<string> categories,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            apiResponse.TraineeshipVacancies.First().RouteId = null;

            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(categories);
            query.AccountLegalEntityPublicHashedId = "";

            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, categories, query.PostedInLastNumberOfDays, query.Sort);
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
            { Standards = new List<GetStandardsListItem> { courseResponse } });

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.TraineeshipVacancies.Where(c => c.RouteId != null).ToList());
            actual.Total.Should().Be(apiResponse.Total);
            actual.TotalFiltered.Should().Be(apiResponse.TotalFound);
            actual.TotalPages.Should().Be((int)Math.Ceiling((decimal)apiResponse.TotalFound / query.PageSize));
        }

        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_Is_Null_Then_No_LegalEntity_Check_Is_Performed(
            GetTraineeshipVacanciesQuery query,
            GetTraineeshipVacanciesResponse apiResponse,
            GetStandardsListResponse courses,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.AccountLegalEntityPublicHashedId = "";
            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, new List<string>(), query.PostedInLastNumberOfDays, query.Sort);
            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(new List<string>());
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            await handler.Handle(query, CancellationToken.None);

            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            providerRelationshipsApiClient.Verify(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_Permission_Checked(
            GetTraineeshipVacanciesQuery query,
            GetTraineeshipVacanciesResponse apiResponse,
            AccountLegalEntityItem accountLegalEntityItem,
            GetStandardsListResponse courses,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            query.Routes = null;
            query.AccountIdentifier = new AccountIdentifier("Employer-ABC123-Product");
            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                query.AccountLegalEntityPublicHashedId, query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, new List<string>(), query.PostedInLastNumberOfDays, query.Sort);
            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(new List<string>());
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            accountLegalEntityPermissionService
                .Setup(x => x.GetAccountLegalEntity(It.Is<AccountIdentifier>(c => c.Equals(query.AccountIdentifier)),
                    query.AccountLegalEntityPublicHashedId)).ReturnsAsync(accountLegalEntityItem);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.TraineeshipVacancies);
        }

        [Test, MoqAutoData]
        public void And_The_AccountLegalEntityPublicHashedId_And_Ukprn_Is_Not_Null_And_AccountPublicHashedId_Is_Null_Then_ProviderRelations_Api_Checked_And_If_Not_In_Response_Exception_Thrown(
            GetTraineeshipVacanciesQuery query,
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
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
            GetTraineeshipVacanciesQuery query,
            GetTraineeshipVacanciesResponse apiResponse,
            GetProviderAccountLegalEntitiesResponse providerAccountLegalEntitiesResponse,
            GetStandardsListResponse courses,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<IAccountLegalEntityPermissionService> accountLegalEntityPermissionService,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            GetTraineeshipVacanciesQueryHandler handler)
        {
            query.Routes = new List<string>();
            query.AccountPublicHashedId = "";
            query.AccountIdentifier = new AccountIdentifier($"External-{externalId}-Product");
            var expectedGetRequest = new GetTraineeshipVacanciesRequest(query.PageNumber, query.PageSize,
                "", query.Ukprn, query.AccountPublicHashedId, query.RouteId,
                query.NationWideOnly, query.Lat, query.Lon, query.DistanceInMiles, new List<string>(), query.PostedInLastNumberOfDays, query.Sort);
            courseService.Setup(x => x.MapRoutesToCategories(query.Routes)).Returns(new List<string>());
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacanciesResponse>(It.Is<GetTraineeshipVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(courses);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.TraineeshipVacancies);
            accountLegalEntityPermissionService
                .Verify(x => x.GetAccountLegalEntity(It.IsAny<AccountIdentifier>(),
                    query.AccountLegalEntityPublicHashedId), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_AccountLegalEntityPublicHashedId_And_No_Account_Or_Ukprn_Then_Exception_Thrown(
                GetTraineeshipVacanciesQuery query,
                GetTraineeshipVacanciesResponse apiResponse,
                AccountDetail accountDetailApiResponse,
                [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApi,
                [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
                [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
                GetTraineeshipVacanciesQueryHandler handler)
        {
            query.AccountPublicHashedId = null;
            query.Ukprn = null;

            Assert.ThrowsAsync<SecurityException>(() => handler.Handle(query, CancellationToken.None));

            providerRelationshipsApiClient.Verify(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()), Times.Never);
            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAllEmployerAccountLegalEntitiesRequest>()), Times.Never);
            apiClient.Verify(x => x.Get<GetTraineeshipVacanciesResponse>(It.IsAny<GetTraineeshipVacanciesRequest>()), Times.Never);
        }
    }
}
