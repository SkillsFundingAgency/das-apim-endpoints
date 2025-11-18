using AutoFixture;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
public class WhenHandlingGetLiveVacancies
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Live_Vacancies(
        GetLiveVacanciesQuery mockQuery,
        GetStandardsListResponse getStandardsListResponse,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockApiClient,
        [Frozen] Mock<ILiveVacancyMapper> mockLiveVacancyMapper,
        GetLiveVacanciesQueryHandler sut)
    {
        var expectedUrl = new GetLiveVacanciesApiRequest(mockQuery.PageNumber, mockQuery.PageSize, mockQuery.ClosingDate);
        var fixture = new Fixture();
        var liveVacancies =  fixture.Build<LiveVacancy>().With(c => c.VacancyType, VacancyType.Apprenticeship).CreateMany().ToList();
        var noType = fixture.Build<LiveVacancy>().Create();
        noType.VacancyType = null;
        liveVacancies.Add(noType);
        var mockApiResponse =
            new ApiResponse<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiResponse { Items = liveVacancies, PageInfo = new PageInfo()
                {
                    TotalCount = liveVacancies.Count,
                    PageIndex = 1,
                    PageSize = 10,
                    TotalPages = 1
                }},
                HttpStatusCode.OK, "");
        mockApiResponse.Body.Items.First().VacancyType = null;
        
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.Is<GetLiveVacanciesApiRequest>(c=>c.GetUrl == expectedUrl.GetUrl))).ReturnsAsync(mockApiResponse);
        courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
            .ReturnsAsync(getStandardsListResponse);

        var mappedVacancies = SetupVacancyMapper(mockLiveVacancyMapper, mockApiResponse.Body.Items, getStandardsListResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, mappedVacancies);
        
    }

    [Test, MoqAutoData]
    public async Task And_Api_Client_Returns_Null(
        GetLiveVacanciesQuery mockQuery,
        ApiResponse<GetLiveVacanciesApiResponse> mockApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> mockApiClient,
        GetLiveVacanciesQueryHandler sut)
    {
        mockApiResponse.Body.Items = [];
        mockApiClient.Setup(client => client.GetWithResponseCode<GetLiveVacanciesApiResponse>(It.IsAny<GetLiveVacanciesApiRequest>())).ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        AssertResponse(actual, Enumerable.Empty<FindApprenticeshipJobs.Application.Shared.LiveVacancy>().ToList());
    }

    private static List<FindApprenticeshipJobs.Application.Shared.LiveVacancy> SetupVacancyMapper(Mock<ILiveVacancyMapper> mapper, IEnumerable<LiveVacancy> vacancies, GetStandardsListResponse standards)
    {
        var fixture = new Fixture();

        var result = new List<FindApprenticeshipJobs.Application.Shared.LiveVacancy>();

        foreach (var vacancy in vacancies)
        {
            var mappedVacancy = fixture.Build< FindApprenticeshipJobs.Application.Shared.LiveVacancy >()
                .With(x => x.VacancyId, vacancy.VacancyId)
                .Create();

            result.Add(mappedVacancy);

            mapper.Setup(x => x.Map(It.Is<LiveVacancy>(v => v == vacancy),standards))
                .Returns(mappedVacancy);
        }

        return result;
    }

    private static void AssertResponse(GetLiveVacanciesQueryResult actual, List<FindApprenticeshipJobs.Application.Shared.LiveVacancy> mockedVacancies)
    {
        foreach (var expectedVacancy in mockedVacancies)
        {
            var expected = actual.Vacancies.Single(x => x.VacancyId == expectedVacancy.VacancyId);
            expectedVacancy.Should().BeEquivalentTo(expected);
        }
    }
}
