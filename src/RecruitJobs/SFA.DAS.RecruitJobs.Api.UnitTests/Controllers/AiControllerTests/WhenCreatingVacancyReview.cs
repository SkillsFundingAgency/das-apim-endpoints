using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Ai;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.AiControllerTests;

public class WhenCreatingVacancyReview
{
    [Test]
    [MoqInlineAutoData(AiReviewStatus.Pending, false)]
    [MoqInlineAutoData(AiReviewStatus.Passed, false)]
    [MoqInlineAutoData(AiReviewStatus.Failed, true)]
    [MoqInlineAutoData(AiReviewStatus.Skipped, true)]
    public async Task Then_The_Vacancy_Review_Record_Should_Be_Created_With_The_Correct_Status(
        AiReviewStatus status,
        bool manualReviewRequired,
        Mock<IRecruitAiApiClient<RecruitAiApiConfiguration>> recruitAiApiClient,
        Guid vacancyId,
        Guid vacancyReviewId,
        [Greedy] AiController sut)
    {
        // arrange
        PutAiVacancyReviewRequest? capturedRequest = null;
        recruitAiApiClient
            .Setup(x => x.PutWithResponseCode<PutAiVacancyReviewDto, NullResponse>(It.IsAny<PutAiVacancyReviewRequest>()))
            .Callback<IPutApiRequest<PutAiVacancyReviewDto>>(x => capturedRequest = x as PutAiVacancyReviewRequest)
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null, null));
        
        var data = new CreateVacancyReviewData(status);

        // act
        var result = await sut.CreateVacancyReviewAsync(recruitAiApiClient.Object, vacancyId, vacancyReviewId, data, CancellationToken.None) as Ok;

        // assert
        result.Should().NotBeNull();
        capturedRequest.Should().NotBeNull();
        capturedRequest.PutUrl.Should().Be($"api/ai-vacancy-reviews/{vacancyReviewId}");
        capturedRequest.Data.Status.Should().Be(status);
        capturedRequest.Data.VacancyId.Should().Be(vacancyId);
        capturedRequest.Data.Output.Should().BeNull();
        capturedRequest.Data.ManualReviewRequired.Should().Be(manualReviewRequired);
    }
}