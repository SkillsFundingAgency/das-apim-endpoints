using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

[TestFixture]
public class WhenHandlingGetLiveVacancy
{
    [Test, MoqAutoData]
    public async Task Then_Vacancy_Is_Returned(
        GetLiveVacancyQuery query,
        ApiResponse<GetLiveVacancyApiResponse> apiResponse,
        GetStandardsListResponse standards,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockVacancyMapper,
        FindApprenticeshipJobs.Application.Shared.LiveVacancy mapperResult,
        GetLiveVacancyQueryHandler sut)
    {
        mockApiClient.Setup(x =>
                x.GetWithResponseCode<GetLiveVacancyApiResponse>(
                    It.Is<GetLiveVacancyApiRequest>(r =>
                        r.GetUrl == $"api/vacancies/{query.VacancyReference}/live")))
            .ReturnsAsync(apiResponse);
        mockCourseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(standards);

        mockVacancyMapper.Setup(x => x.Map(It.Is<LiveVacancy>(v => v == apiResponse.Body), standards))
            .Returns(mapperResult);

        var result = await sut.Handle(query, CancellationToken.None);

        result.LiveVacancy.Should().BeEquivalentTo(mapperResult);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Request_Is_Retried_And_Vacancy_Is_Returned(
        GetLiveVacancyQuery query,
        ApiResponse<GetLiveVacancyApiResponse> apiResponse,
        GetStandardsListResponse standards,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockVacancyMapper,
        FindApprenticeshipJobs.Application.Shared.LiveVacancy mapperResult,
        GetLiveVacancyQueryHandler sut)
    {
        mockApiClient.SetupSequence(x =>
                x.GetWithResponseCode<GetLiveVacancyApiResponse>(
                    It.Is<GetLiveVacancyApiRequest>(r =>
                        r.GetUrl == $"api/vacancies/{query.VacancyReference}/live")))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(null!, HttpStatusCode.NotFound, ""))
            .ReturnsAsync(apiResponse);
        mockCourseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(standards);

        mockVacancyMapper.Setup(x => x.Map(It.Is<LiveVacancy>(v => v == apiResponse.Body), standards))
            .Returns(mapperResult);

        var result = await sut.Handle(query, CancellationToken.None);

        result.LiveVacancy.Should().BeEquivalentTo(mapperResult);
    }
    
    [Test, MoqAutoData]
    public void Then_If_Not_Found_Exception_Is_Thrown(
        GetLiveVacancyQuery query,
        ApiResponse<GetLiveVacancyApiResponse> apiResponse,
        GetStandardsListResponse standards,
        [Frozen] Mock<ICourseService> mockCourseService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockVacancyMapper,
        FindApprenticeshipJobs.Application.Shared.LiveVacancy mapperResult,
        GetLiveVacancyQueryHandler sut)
    {
        mockApiClient.Setup(x =>
                x.GetWithResponseCode<GetLiveVacancyApiResponse>(
                    It.Is<GetLiveVacancyApiRequest>(r =>
                        r.GetUrl == $"api/vacancies/{query.VacancyReference}/live")))
            .ReturnsAsync(new ApiResponse<GetLiveVacancyApiResponse>(null!, HttpStatusCode.NotFound, ""));
        mockCourseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(standards);

        mockVacancyMapper.Setup(x => x.Map(It.Is<LiveVacancy>(v => v == apiResponse.Body), standards))
            .Returns(mapperResult);

        Assert.ThrowsAsync<Exception>(()=>sut.Handle(query, CancellationToken.None));
    }
}