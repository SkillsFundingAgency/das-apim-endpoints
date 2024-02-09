using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingCreateTrainingCourseCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResponse_Is_Returned(
        CreateTrainingCourseCommand command,
        PostTrainingCourseApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        CreateTrainingCourseCommandHandler handler)
    {
        var expectedRequest = new PostTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, new PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData());
        candidateApiClient.Setup(client => client.PostWithResponseCode<PostTrainingCourseApiResponse>(It.Is<PostTrainingCourseApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
            .ReturnsAsync(new ApiResponse<PostTrainingCourseApiResponse>(apiResponse, System.Net.HttpStatusCode.Created, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Id.Should().Be(apiResponse.Id);
    }
}
