using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application
{
    [TestFixture]
    public class WhenHandlingGetLiveVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_Vacancy_Is_Returned(
            GetLiveVacancyQuery query,
            ApiResponse<GetLiveVacancyApiResponse> apiResponse,
            GetStandardsListResponse standards,
            [Frozen] Mock<ICourseService> mockCourseService,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> mockApiClient,
            [Frozen] Mock<ILiveVacancyMapper> mockVacancyMapper,
            FindApprenticeshipJobs.Application.Shared.LiveVacancy mapperResult,
            GetLiveVacancyQueryHandler sut)
        {
            mockApiClient.Setup(x =>
                    x.GetWithResponseCode<GetLiveVacancyApiResponse>(
                        It.Is<GetLiveVacancyApiRequest>(r =>
                            r.GetUrl == $"api/livevacancies/{query.VacancyReference}")))
                .ReturnsAsync(apiResponse);
            mockCourseService
                .Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(standards);

            mockVacancyMapper.Setup(x => x.Map(It.Is<LiveVacancy>(v => v == apiResponse.Body), standards))
                .Returns(mapperResult);

            var result = await sut.Handle(query, CancellationToken.None);

            result.LiveVacancy.Should().BeEquivalentTo(mapperResult);
        }
    }
}
