using System;
using System.Linq;
using System.Net;
using System.Threading;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.UnitTests.Ai;

public class WhenReviewingVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Result_Should_Be_False_When_The_Training_Course_Cannot_Be_Found(
        RecruitAiService.GetStandardsListResponse coursesResponse,
        Vacancy vacancy,
        [Frozen] Mock<ICourseService> courseService,
        [Greedy] RecruitAiService sut)
    {
        // arrange
        courseService
            .Setup(x => x.GetActiveStandards<RecruitAiService.GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(coursesResponse);

        // act
        var result = await sut.ReviewVacancyAsync(Guid.NewGuid(), vacancy, CancellationToken.None);

        // assert
        result.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Data_Is_Sent_For_Review(
        RecruitAiService.GetStandardsListResponse coursesResponse,
        Vacancy vacancy,
        [Frozen] Mock<ICourseService> courseService,
        [Frozen] Mock<IRecruitAiApiClient<RecruitAiApiConfiguration>> recruitAiApiClient,
        [Greedy] RecruitAiService sut)
    {
        // arrange
        var programme = coursesResponse.Standards.First();
        vacancy.ProgrammeId = $"{programme.LarsCode}";
        courseService
            .Setup(x => x.GetActiveStandards<RecruitAiService.GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(coursesResponse);

        PostVacancyReviewRequest? capturedRequest = null;
        recruitAiApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostVacancyReviewRequest>(), false))
            .Callback<IPostApiRequest, bool>((x, _) => { capturedRequest = x as PostVacancyReviewRequest; })
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, null));
        
        // act
        var result = await sut.ReviewVacancyAsync(Guid.NewGuid(), vacancy, CancellationToken.None);
        var data = capturedRequest?.Data as PostVacancyReviewDto;

        // assert
        result.Should().BeTrue();
        capturedRequest.Should().NotBeNull();
        data.Should().NotBeNull();
        vacancy.Should().BeEquivalentTo(data, opt => opt
            .WithMapping<PostVacancyReviewDto, Vacancy>(x => x.VacancyId, x => x.Id)
            .ExcludingMissingMembers()
        );
        data.TrainingProgrammeTitle.Should().Be(programme.Title);
        data.TrainingProgrammeLevel.Should().Be($"Level {programme.Level}");
        data.WageAdditionalInformation.Should().Be(vacancy.Wage!.WageAdditionalInformation);
        data.WageCompanyBenefitsInformation.Should().Be(vacancy.Wage.CompanyBenefitsInformation);
        data.WageWorkingWeekDescription.Should().Be(vacancy.Wage.WorkingWeekDescription);


    }
}