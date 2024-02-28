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
    public class WhenValidatingChangeOfEmployerOverlap
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_A_Valid_Request(
          ValidateChangeOfEmployerOverlapCommand query,
          [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
          ValidateChangeOfEmployerOverlapCommandHandler handler
          )
        {
            var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.OK, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostValidateChangeOfEmployerOverlapRequest>(), false)).ReturnsAsync(response);
            var actual = await handler.Handle(query, CancellationToken.None);
            Assert.That(actual, Is.Not.Null);
        }

        [Test, MoqAutoData]
        public void Then_The_Api_Is_Called_And_Not_Valid_Response_Is_Received(
         ValidateChangeOfEmployerOverlapCommand query,
         [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> apiClient,
         ValidateChangeOfEmployerOverlapCommandHandler handler
         )
        {
            var response = new ApiResponse<object>(null, System.Net.HttpStatusCode.InternalServerError, string.Empty);
            apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostValidateChangeOfEmployerOverlapRequest>(), false)).ReturnsAsync(response);
            Assert.CatchAsync<ApiResponseException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}
