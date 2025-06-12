using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Command
{
    [TestFixture]
    public class WhenHandlingPatchApplicationReviewCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_Success(
         PatchApplicationReviewCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
         PatchApplicationReviewCommandHandler handler)
        {
            recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
                .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

            await handler.Handle(request, CancellationToken.None);
            
            recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
                    c.PatchUrl.Contains(request.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&

                    c.Data.Operations[0].path == "/Status" &&
                    c.Data.Operations[0].value.ToString() == request.Status &&

                    c.Data.Operations[1].path == "/HasEverBeenEmployerInterviewing" &&
                    c.Data.Operations[1].value.ToString() == request.HasEverBeenEmployerInterviewing.ToString() &&

                    c.Data.Operations[3].path == "/EmployerFeedback" &&
                    c.Data.Operations[3].value.ToString() == request.EmployerFeedback &&

                    c.Data.Operations[4].path == "/TemporaryReviewStatus" &&
                    c.Data.Operations[4].value.ToString() == request.TemporaryReviewStatus && 
                    
                    c.Data.Operations[5].path == "/DateSharedWithEmployer" &&
                    c.Data.Operations[5].value.ToString() == request.DateSharedWithEmployer!.Value.ToString()
                )), Times.Once
            );
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
            PatchApplicationReviewCommand request,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            PatchApplicationReviewCommandHandler handler)
        {
            var expectedPatchRequest = new PatchRecruitApplicationReviewApiRequest(request.Id, new JsonPatchDocument<ApplicationReview>());

            recruitApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, string.Empty));

            var act = async () => { await handler.Handle(request, CancellationToken.None); };
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}