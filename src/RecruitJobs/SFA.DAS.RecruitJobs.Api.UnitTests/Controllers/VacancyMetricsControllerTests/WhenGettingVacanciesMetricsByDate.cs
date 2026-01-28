using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.VacancyMetrics;
using SFA.DAS.RecruitJobs.InnerApi.Responses.VacancyMetrics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyMetricsControllerTests;

[TestFixture]
internal class WhenGettingVacanciesMetricsByDate
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
        DateTime startDate,
        DateTime endDate,
        GetVacancyMetricsByDateResponse response,
        Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> businessMetricApiClient,
        [Greedy] MetricsController sut)
    {
        // arrange
        GetVacancyMetricsByDateRequest? capturedRequest = null;
        businessMetricApiClient
            .Setup(x => x.Get<GetVacancyMetricsByDateResponse>(It.IsAny<GetVacancyMetricsByDateRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetVacancyMetricsByDateRequest)
            .ReturnsAsync(response);

        const string baseUrl = "api/vacancies/metrics";
        var queryParams = new Dictionary<string, string>
        {
            { "startDate", startDate.ToString("s") },
            { "endDate", endDate.ToString("s") }
        };
        var expectedUrl = QueryHelpers.AddQueryString(baseUrl, queryParams);

        // act
        await sut.GetByDate(businessMetricApiClient.Object, startDate, endDate);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be(expectedUrl);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        DateTime startDate,
        DateTime endDate,
        GetVacancyMetricsByDateResponse response,
        Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> businessMetricApiClient,
        [Greedy] MetricsController sut)
    {
        // arrange
        businessMetricApiClient
            .Setup(x => x.Get<GetVacancyMetricsByDateResponse>(It.IsAny<GetVacancyMetricsByDateRequest>()))
            .Callback<IGetApiRequest>(x => _ = x as GetVacancyMetricsByDateRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.GetByDate(businessMetricApiClient.Object, startDate, endDate) as Ok<GetVacancyMetricsByDateResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Response_Then_Empty_Results_Are_Returned(
        DateTime startDate,
        DateTime endDate,
        GetVacancyMetricsByDateResponse response,
        Mock<IBusinessMetricsApiClient<BusinessMetricsConfiguration>> businessMetricApiClient,
        [Greedy] MetricsController sut)
    {
        // arrange
        businessMetricApiClient
            .Setup(x => x.Get<GetVacancyMetricsByDateResponse>(It.IsAny<GetVacancyMetricsByDateRequest>()))
            .ReturnsAsync((GetVacancyMetricsByDateResponse)null!);

        // act
        var result = await sut.GetByDate(businessMetricApiClient.Object, startDate, endDate) as Ok<GetVacancyMetricsByDateResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new GetVacancyMetricsByDateResponse());
    }
}
