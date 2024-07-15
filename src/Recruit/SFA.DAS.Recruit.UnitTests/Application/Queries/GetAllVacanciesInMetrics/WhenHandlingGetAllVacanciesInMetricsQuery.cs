using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetAllVacanciesInMetrics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAllVacanciesInMetrics
{
    public class WhenHandlingGetAllVacanciesInMetricsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetAllVacanciesInMetricsQuery query,
            GetAllVacanciesResponse apiResponse,
            [Frozen] Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> metricsApiClient,
            GetAllVacanciesInMetricsQueryHandler handler)
        {
            //Arrange
            var expectedVacancyGetRequest = new GetAllVacanciesRequest(query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetAllVacanciesResponse>(
                        It.Is<GetAllVacanciesRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Vacancies.Should().BeEquivalentTo(apiResponse.Vacancies);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned_When_Null(
            GetAllVacanciesInMetricsQuery query,
            GetAllVacanciesResponse apiResponse,
            [Frozen] Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> metricsApiClient,
            GetAllVacanciesInMetricsQueryHandler handler)
        {
            //Arrange
            apiResponse.Vacancies = [];
            var expectedVacancyGetRequest = new GetAllVacanciesRequest(query.StartDate, query.EndDate);
            metricsApiClient.Setup(x =>
                    x.Get<GetAllVacanciesResponse>(
                        It.Is<GetAllVacanciesRequest>(c => c.GetUrl.Equals(expectedVacancyGetRequest.GetUrl))))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Vacancies.Count.Should().Be(0);
        }
    }
}
