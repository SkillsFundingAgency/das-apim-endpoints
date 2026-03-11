using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyAnalytics;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.VacancyAnalytics;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Vacancies;

[TestFixture]
internal class WhenGettingVacancyAnalytics
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Sent_Correctly(
       long vacancyReference,
       GetOneVacancyAnalyticsResponse response,
       Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
       [Greedy] VacanciesController sut)
    {
        // arrange
        GetOneVacancyAnalyticsApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.Get<GetOneVacancyAnalyticsResponse>(It.IsAny<GetOneVacancyAnalyticsApiRequest>()))
            .Callback<IGetApiRequest>(x => capturedRequest = x as GetOneVacancyAnalyticsApiRequest)
            .ReturnsAsync(response);

        var expectedUrl = $"api/vacancyAnalytics/{vacancyReference}";

        // act
        await sut.GetOne(recruitApiClient.Object, vacancyReference);

        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.GetUrl.Should().Be(expectedUrl);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        long vacancyReference,
        GetOneVacancyAnalyticsResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetOneVacancyAnalyticsResponse>(It.IsAny<GetOneVacancyAnalyticsApiRequest>()))
            .Callback<IGetApiRequest>(x => _ = x as GetOneVacancyAnalyticsApiRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<GetOneVacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Response_Then_Null_Results_Are_Returned(
        long vacancyReference,
        GetOneVacancyAnalyticsResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetOneVacancyAnalyticsResponse>(It.IsAny<GetOneVacancyAnalyticsApiRequest>()))
            .ReturnsAsync((GetOneVacancyAnalyticsResponse)null!);

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<GetOneVacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Empty_Response_Then_Empty_Results_Are_Returned(
        long vacancyReference,
        GetOneVacancyAnalyticsResponse response,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<GetOneVacancyAnalyticsResponse>(It.IsAny<GetOneVacancyAnalyticsApiRequest>()))
            .ReturnsAsync(new GetOneVacancyAnalyticsResponse()
            {
                VacancyReference = vacancyReference,
                Analytics = []
            });

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<GetOneVacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.VacancyReference.Should().Be(vacancyReference);
        result!.Value.Analytics.Should().BeEmpty();
    }
}