using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using ReviewStatus = SFA.DAS.SharedOuterApi.Types.Domain.Recruit.ReviewStatus;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.AiControllerTests;

public class WhenAutoApprovingVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Review_Is_Updated(
        Guid vacancyId,
        Guid vacancyReviewId,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] AiController sut)
    {
        // arrange
        PatchVacancyReviewRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyReviewDto>, NullResponse>(
                It.IsAny<IPatchApiRequest<JsonPatchDocument<PatchableVacancyReviewDto>>>(), false))
            .Callback<IPatchApiRequest<JsonPatchDocument<PatchableVacancyReviewDto>>, bool>((x, _) => capturedRequest = x as PatchVacancyReviewRequest)
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.OK, null));

        // act
        var response = await sut.AutoApproveVacancyAsync(recruitApiClient.Object, vacancyId, vacancyReviewId, CancellationToken.None);

        // assert
        response.Should().BeOfType<NoContent>();
        capturedRequest.Should().NotBeNull();
        capturedRequest.PatchUrl.Should().Be($"api/vacancyreviews/{vacancyReviewId}");
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<PatchableVacancyReviewDto>("replace", "/ManualOutcome", null, nameof(ManualQaOutcome.Bypassed)));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<PatchableVacancyReviewDto>("replace", "/Status", null, ReviewStatus.Closed));
        var datetime = Convert.ToDateTime(capturedRequest.Data.Operations.Find(x => x.path == "/ClosedDate").value);
        datetime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}