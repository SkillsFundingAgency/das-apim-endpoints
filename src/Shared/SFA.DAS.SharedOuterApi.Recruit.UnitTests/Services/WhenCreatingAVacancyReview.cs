using System.Net;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Recruit.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests.Services;

public class WhenCreatingAVacancyReview
{
    [Test, MoqAutoData]
    public async Task Then_An_Existing_Non_Closed_Review_Aborts_The_Vacancy_Review_Record_Creation(
        Vacancy vacancy,
        string submittingUserEmailAddress,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IVacancySlaDeadlineService> vacancySlaDeadlineService,
        [Frozen] Mock<IVacancyComparerService> vacancyComparerService,
        VacancyReviewService sut)
    {
        // arrange
        GetVacancyReviewsByVacancyReferenceRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.GetAll<VacancyReview>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()))
            .Callback<IGetAllApiRequest>(x => capturedRequest = x as GetVacancyReviewsByVacancyReferenceRequest)
            .ReturnsAsync([new VacancyReview { Status = ReviewStatus.New }]);

        // act
        var result = await sut.CreateAsync(vacancy, submittingUserEmailAddress);

        // assert
        result.Should().BeNull();
        capturedRequest.Should().NotBeNull();
        capturedRequest.GetAllUrl.Should().Contain($"{vacancy.VacancyReference}");
        vacancySlaDeadlineService.Verify(x => x.GetSlaDeadlineAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task Then_A_Vacancy_Review_Record_Is_Created(
        Vacancy vacancy,
        string submittingUserEmailAddress,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IVacancySlaDeadlineService> vacancySlaDeadlineService,
        [Frozen] Mock<IVacancyComparerService> vacancyComparerService,
        VacancyReviewService sut)
    {
        // arrange
        recruitApiClient
            .Setup(x => x.GetAll<VacancyReview>(It.IsAny<GetVacancyReviewsByVacancyReferenceRequest>()))
            .ReturnsAsync([]);

        var expectedSlaDeadLine = DateTime.UtcNow.AddDays(7);
        vacancySlaDeadlineService
            .Setup(x => x.GetSlaDeadlineAsync(vacancy.SubmittedDate.GetValueOrDefault(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedSlaDeadLine);

        PutVacancyReviewRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<IPutApiRequest>()))
            .Callback<IPutApiRequest>(x => capturedRequest = x as PutVacancyReviewRequest)
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null!));

        // act
        var result = await sut.CreateAsync(vacancy, submittingUserEmailAddress);

        // assert
        result.Should().NotBeNull();
        capturedRequest.Should().NotBeNull();
        capturedRequest.PutUrl.Should().Contain(new PutVacancyReviewRequest(result.Id, null).PutUrl);
        result.SlaDeadLine.Should().Be(expectedSlaDeadLine);
    }
}