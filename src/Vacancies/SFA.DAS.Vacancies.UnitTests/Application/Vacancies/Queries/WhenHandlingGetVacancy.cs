using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Enums;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancy_And_Course_Returned(
            Guid vacancyReference,
            GetVacancyQuery query,
            GetVacancyApiResponse apiApiResponse,
            GetStandardsListItem courseResponse,
            string findAnApprenticeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<ICourseService> standardsService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacancyQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            courseResponse.LarsCode = apiApiResponse.StandardLarsCode!.Value;
            standardsService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
                { Standards = new List<GetStandardsListItem> { courseResponse } });
            
            var expectedGetRequest = new GetVacancyRequest(query.VacancyReference);
            apiClient.Setup(x =>
                x.Get<GetVacancyApiResponse>(It.Is<GetVacancyRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiApiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeEquivalentTo(apiApiResponse);
            actual.Vacancy.VacancyUrl.Should()
                .Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{actual.Vacancy.VacancyReference.TrimVacancyReference()}");
            actual.Vacancy.CourseLevel.Should().Be(courseResponse.Level);
            actual.Vacancy.CourseTitle.Should().Be(courseResponse.Title);
            actual.Vacancy.Route.Should().Be(courseResponse.Route);
        }
        
        [Test]
        [MoqInlineAutoData(DataSource.Nhs)]
        [MoqInlineAutoData(DataSource.Csj)]
        public async Task Then_The_Vacancy_Url_Is_Correct_For_Non_Raa_Vacancy(DataSource dataSource,
            Guid vacancyReference,
            GetVacancyQuery query,
            GetVacancyApiResponse vacancyApiResponse,
            GetStandardsListItem courseResponse,
            string findAnApprenticeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<ICourseService> standardsService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacancyQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            courseResponse.LarsCode = vacancyApiResponse.StandardLarsCode!.Value;
            vacancyApiResponse.VacancySource = dataSource;
            vacancyApiResponse.VacancyReference = vacancyReference.ToString();
            standardsService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
                { Standards = new List<GetStandardsListItem> { courseResponse } });
            
            var expectedGetRequest = new GetVacancyRequest(query.VacancyReference);
            apiClient.Setup(x =>
                x.Get<GetVacancyApiResponse>(It.Is<GetVacancyRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(vacancyApiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeEquivalentTo(vacancyApiResponse);
            actual.Vacancy.VacancyUrl.Should()
                .Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancyApiResponse.VacancyReference.Replace("VAC","", StringComparison.CurrentCultureIgnoreCase)}");
            actual.Vacancy.CourseLevel.Should().Be(courseResponse.Level);
            actual.Vacancy.CourseTitle.Should().Be(courseResponse.Title);
            actual.Vacancy.Route.Should().Be(courseResponse.Route);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancy_And_Course_Not_looked_Up_If_Standard_Is_Null(
            GetVacancyQuery query,
            GetVacancyApiResponse apiApiResponse,
            GetStandardsListItem courseResponse,
            string findAnApprenticeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<ICourseService> standardsService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacancyQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            apiApiResponse.StandardLarsCode = null;
            standardsService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(new GetStandardsListResponse
                { Standards = new List<GetStandardsListItem> { courseResponse } });
            
            var expectedGetRequest = new GetVacancyRequest(query.VacancyReference);
            apiClient.Setup(x =>
                x.Get<GetVacancyApiResponse>(It.Is<GetVacancyRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiApiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeEquivalentTo(apiApiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Vacancy_Not_Found_Then_Null_Returned(
            GetVacancyQuery query,
            GetVacancyApiResponse apiApiResponse,
            GetStandardsListItem courseResponse,
            string findAnApprenticeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetVacancyQueryHandler handler)
        {
            apiClient.Setup(x =>
                x.Get<GetVacancyApiResponse>(It.IsAny<GetVacancyRequest>())).ReturnsAsync((GetVacancyApiResponse)null);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeNull();
        }
    }
}