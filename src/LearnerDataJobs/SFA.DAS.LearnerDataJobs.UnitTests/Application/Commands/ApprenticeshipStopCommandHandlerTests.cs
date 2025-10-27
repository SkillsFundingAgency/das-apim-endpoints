using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Commands
{
    public class ApprenticeshipStopCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Then_StopApprenticeship_Returns_false_when_learnerbodyisnull(
        ApprenticeshipStopCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] ApprenticeshipStopCommandHandler handler)
        {
            var expectedGetUrl =
              $"providers/{command.ProviderId}/learners/{command.LearnerDataId}";

            client.Setup(x =>
                    x.GetWithResponseCode<GetLearnerDataByIdResponse>(
                        It.Is<GetLearnerByIdRequest>(p => p.GetUrl == expectedGetUrl)))
                 .ReturnsAsync((ApiResponse<GetLearnerDataByIdResponse>)null);


            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
        }



        [Test, MoqAutoData]
        public async Task Then_StopApprenticeship_Returns_200_Range_HttpStatusCode(
        ApprenticeshipStopCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] ApprenticeshipStopCommandHandler handler)
        {
            command.PatchRequest.IsWithDrawnAtStartOfCourse = true;

            var expectedGetUrl =
              $"providers/{command.ProviderId}/learners/{command.LearnerDataId}";

            client.Setup(x =>
                    x.GetWithResponseCode<GetLearnerDataByIdResponse>(
                        It.Is<GetLearnerByIdRequest>(p => p.GetUrl == expectedGetUrl)))
                 .ReturnsAsync(new ApiResponse<GetLearnerDataByIdResponse>(new GetLearnerDataByIdResponse() 
                 { ApprenticeshipId = command.PatchRequest.ApprenticeshipId }, HttpStatusCode.OK, "")); 

            var expectedUrl =
          $"providers/{command.ProviderId}/learners/{command.LearnerDataId}/apprenticeshipId";
            client.Setup(x =>
                    x.PatchWithResponseCode(
                        It.Is<PatchLearnerDataApprenticeshipIdRequest>(p =>p.PatchUrl == expectedUrl)))
                .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, "",null));

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeTrue();
        }
    }
}
