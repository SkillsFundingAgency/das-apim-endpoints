using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyControllerTests;

[TestFixture]
internal class WhenGettingVacancyAnalytics
{
    [Test, MoqAutoData]
    public async Task Then_The_Results_Are_Returned(
        long vacancyReference,
        VacancyAnalyticsResponse response,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<VacancyAnalyticsResponse>(It.IsAny<GetVacancyanalyticsByVacancyReferenceApiRequest>()))
            .Callback<IGetApiRequest>(x => _ = x as GetVacancyanalyticsByVacancyReferenceApiRequest)
            .ReturnsAsync(response);

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<VacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Response_Then_Null_Results_Are_Returned(
        long vacancyReference,
        VacancyAnalyticsResponse response,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<VacancyAnalyticsResponse>(It.IsAny<GetVacancyanalyticsByVacancyReferenceApiRequest>()))
            .ReturnsAsync((VacancyAnalyticsResponse)null!);

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<VacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Empty_Response_Then_Empty_Results_Are_Returned(
        long vacancyReference,
        VacancyAnalyticsResponse response,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.Get<VacancyAnalyticsResponse>(It.IsAny<GetVacancyanalyticsByVacancyReferenceApiRequest>()))
            .ReturnsAsync(new VacancyAnalyticsResponse()
            {
                VacancyReference = vacancyReference,
                Analytics = []
            });

        // act
        var result = await sut.GetOne(recruitApiClient.Object, vacancyReference) as Ok<VacancyAnalyticsResponse>;

        // assert
        result.Should().NotBeNull();
        result!.Value.VacancyReference.Should().Be(vacancyReference);
        result!.Value.Analytics.Should().BeEmpty();
    }
}