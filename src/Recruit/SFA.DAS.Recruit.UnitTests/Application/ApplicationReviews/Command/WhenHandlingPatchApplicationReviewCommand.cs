using System.Net;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Command;

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
    
    [Test]
    [MoqInlineAutoData("")]
    [MoqInlineAutoData(" ")]
    [MoqInlineAutoData(null)]
    public async Task Then_The_Temporary_Status_Is_Set_To_Null(
        string? temporaryStatus,
        PatchApplicationReviewCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        PatchApplicationReviewCommandHandler handler)
    {
        // arrange
        command = command with
        {
            TemporaryReviewStatus = temporaryStatus 
        };
        
        var expectedPatchRequest = new PatchRecruitApplicationReviewApiRequest(command.Id, new JsonPatchDocument<ApplicationReview>());

        PatchRecruitApplicationReviewApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .Callback<IPatchApiRequest<JsonPatchDocument<ApplicationReview>>>(x => capturedRequest = x as PatchRecruitApplicationReviewApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        
        // act
        await handler.Handle(command, CancellationToken.None);
        
        // assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Data.Operations.Should().Contain(x => x.path == "/TemporaryReviewStatus" && x.value == null);
    }
}