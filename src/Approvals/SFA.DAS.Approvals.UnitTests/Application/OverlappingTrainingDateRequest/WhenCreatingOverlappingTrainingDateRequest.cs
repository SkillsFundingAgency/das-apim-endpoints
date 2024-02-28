using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.OverlappingTrainingDateRequest
{
    public class WhenCreatingOverlappingTrainingDateRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
         CreateOverlappingTrainingDateRequestCommand query,
         [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
         CreateOverlappingTrainingDateRequestCommandHandler handler
         )
        {
            var response = new ApiResponse<CreateOverlappingTrainingDateResult>(new CreateOverlappingTrainingDateResult { Id = 1 }, System.Net.HttpStatusCode.OK, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<CreateOverlappingTrainingDateResult>(It.IsAny<PostCreateOverlappingTrainingDateRequest>(), true)).ReturnsAsync(response);
            var actual = await handler.Handle(query, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);
        }

        [Test, MoqAutoData]
        public void Then_The_Api_Is_Called_And_Invlaid_Response_Received(
        CreateOverlappingTrainingDateRequestCommand query,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
        CreateOverlappingTrainingDateRequestCommandHandler handler
        )
        {
            var response = new ApiResponse<CreateOverlappingTrainingDateResult>(new CreateOverlappingTrainingDateResult { Id = 1 }, System.Net.HttpStatusCode.InternalServerError, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<CreateOverlappingTrainingDateResult>(It.IsAny<PostCreateOverlappingTrainingDateRequest>(), true)).ReturnsAsync(response);
            Assert.CatchAsync<ApiResponseException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_Correct_Parameters(
      CreateOverlappingTrainingDateRequestCommand cmd,
      [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
      CreateOverlappingTrainingDateRequestCommandHandler handler
      )
        {
            var response = new ApiResponse<CreateOverlappingTrainingDateResult>(new CreateOverlappingTrainingDateResult { Id = 1 }, System.Net.HttpStatusCode.OK, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<CreateOverlappingTrainingDateResult>(It.Is<PostCreateOverlappingTrainingDateRequest>(x => x.ProviderId == cmd.ProviderId
            && (x.Data as CreateOverlappingTrainingDateRequest).DraftApprenticeshipId == cmd.DraftApprenticeshipId
            && (x.Data as CreateOverlappingTrainingDateRequest).UserInfo == cmd.UserInfo
                ), true)).ReturnsAsync(response);
            var actual = await handler.Handle(cmd, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);
        }
    }
}
