using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;
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
            var expectedVacancyGetRequest = new GetVacancyMetricsRequest(query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetVacancyMetricsResponse>(
                        It.Is<GetVacancyMetricsRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync(vacancyApiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.VacancyMetrics.Should().BeEquivalentTo(vacancyApiResponse.VacancyMetrics);
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
            var expectedVacancyGetRequest = new GetVacancyMetricsRequest(query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetVacancyMetricsResponse>(
                        It.Is<GetVacancyMetricsRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync((GetVacancyMetricsResponse)null!);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.VacancyMetrics.Count.Should().Be(0);
        }
    }
}
