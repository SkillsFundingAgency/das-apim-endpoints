using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetTraineeshipVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancy_Returned(
            GetTraineeshipVacancyQuery query,
            GetTraineeshipVacancyApiResponse apiApiResponse,
            string findATraineeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetTraineeshipVacancyQueryHandler handler)
        {
            vacanciesConfiguration.Object.Value.FindATraineeshipBaseUrl = findATraineeshipBaseUrl;

            var expectedGetRequest = new GetTraineeshipVacancyRequest(query.VacancyReference);
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacancyApiResponse>(It.Is<GetTraineeshipVacancyRequest>(c =>
                    c.GetUrl.Equals(expectedGetRequest.GetUrl)))).ReturnsAsync(apiApiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeEquivalentTo(apiApiResponse);
            actual.Vacancy.VacancyUrl.Should()
                .Be($"{findATraineeshipBaseUrl}/traineeship/reference/{actual.Vacancy.VacancyReference}");
        }

        [Test, MoqAutoData]
        public async Task Then_If_Vacancy_Not_Found_Then_Null_Returned(
            GetTraineeshipVacancyQuery query,
            GetTraineeshipVacancyApiResponse apiApiResponse,
            GetStandardsListItem courseResponse,
            string findAnApprenticeshipBaseUrl,
            List<string> categories,
            [Frozen] Mock<IFindTraineeshipApiClient<FindTraineeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IOptions<VacanciesConfiguration>> vacanciesConfiguration,
            GetTraineeshipVacancyQueryHandler handler)
        {
            apiClient.Setup(x =>
                x.Get<GetTraineeshipVacancyApiResponse>(It.IsAny<GetTraineeshipVacancyRequest>())).ReturnsAsync((GetTraineeshipVacancyApiResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancy.Should().BeNull();
        }
    }
}