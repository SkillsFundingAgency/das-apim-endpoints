using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetVacancyMetrics
{
    public class WhenHandlingVacancyMetricsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetVacancyMetricsQuery query,
            GetVacancyMetricsResponse vacancyApiResponse,
            GetVacancyMetricsResponse findAnApprenticeshipApiResponse,
            [Frozen] Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> metricsApiClient,
            GetVacancyMetricsQueryHandler handler)
        {
            //Arrange
            var expectedVacancyGetRequest = new GetVacancyMetricsRequest(nameof(BusinessMetricServiceNames.VacanciesOuterApi), query.VacancyReference, query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetVacancyMetricsResponse>(
                        It.Is<GetVacancyMetricsRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync(vacancyApiResponse);

            var expectedFindAnApprenticeshipGetRequest = new GetVacancyMetricsRequest(nameof(BusinessMetricServiceNames.FindAnApprenticeshipOuterApi), query.VacancyReference, query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetVacancyMetricsResponse>(
                        It.Is<GetVacancyMetricsRequest>(c => c.GetUrl.Equals(expectedFindAnApprenticeshipGetRequest.GetUrl))))
                .ReturnsAsync(findAnApprenticeshipApiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ViewsCount.Should().Be(vacancyApiResponse.ViewsCount + findAnApprenticeshipApiResponse.ViewsCount);
            actual.ApplicationStartedCount.Should().Be(vacancyApiResponse.ApplicationStartedCount + findAnApprenticeshipApiResponse.ApplicationStartedCount);
            actual.ApplicationSubmittedCount.Should().Be(vacancyApiResponse.ApplicationSubmittedCount + findAnApprenticeshipApiResponse.ApplicationSubmittedCount);
            actual.SearchResultsCount.Should().Be(vacancyApiResponse.SearchResultsCount + findAnApprenticeshipApiResponse.SearchResultsCount);
        }

        [Test, MoqAutoData]
        public async Task Then_If_NotFound_Response_Then_Empty_Returned(
            GetVacancyMetricsQuery query,
            GetVacancyMetricsResponse vacancyApiResponse,
            GetVacancyMetricsResponse findAnApprenticeshipApiResponse,
            [Frozen] Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> metricsApiClient,
            GetVacancyMetricsQueryHandler handler)
        {
            //Arrange
            var expectedVacancyGetRequest = new GetVacancyMetricsRequest(nameof(BusinessMetricServiceNames.VacanciesOuterApi), query.VacancyReference, query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetVacancyMetricsResponse>(
                        It.Is<GetVacancyMetricsRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync((GetVacancyMetricsResponse)null!);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.ViewsCount.Should().Be(0);
            actual.ApplicationStartedCount.Should().Be(0);
            actual.ApplicationSubmittedCount.Should().Be(0);
            actual.SearchResultsCount.Should().Be(0);
        }
    }
}
