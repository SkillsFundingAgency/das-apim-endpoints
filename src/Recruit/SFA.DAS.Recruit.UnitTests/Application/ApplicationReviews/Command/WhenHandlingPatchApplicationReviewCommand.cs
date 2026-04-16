using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Command
{
    [TestFixture]
    public class WhenHandlingPatchApplicationReviewCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_Success(
         PatchApplicationReviewCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
         PatchApplicationReviewCommandHandler handler)
        {
            recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

            await handler.Handle(command, CancellationToken.None);

            recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
            PatchApplicationReviewCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            PatchApplicationReviewCommandHandler handler)
        {
            var expectedPatchRequest = new PatchRecruitApplicationReviewApiRequest(command.Id, new JsonPatchDocument<ApplicationReview>());

            recruitApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.NotFound, string.Empty));

            await handler.Handle(command, CancellationToken.None);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Api_Response_IsNotSuccess_CommandResult_Is_Returned_As_Expected(
            PatchApplicationReviewCommand command,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            PatchApplicationReviewCommandHandler handler)
        {
            var expectedPatchRequest = new PatchRecruitApplicationReviewApiRequest(command.Id, new JsonPatchDocument<ApplicationReview>());

            recruitApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.InternalServerError, string.Empty));

            var act = async () => { await handler.Handle(command, CancellationToken.None); };
            await act.Should().ThrowAsync<ApiResponseException>();

        }
        
    }
}