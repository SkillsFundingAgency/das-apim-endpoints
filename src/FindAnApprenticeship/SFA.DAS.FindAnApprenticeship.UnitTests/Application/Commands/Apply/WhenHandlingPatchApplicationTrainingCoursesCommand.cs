using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingPatchApplicationTrainingCoursesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            PatchApplicationTrainingCoursesCommand command,
            FindAnApprenticeship.Domain.Models.Application candidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            PatchApplicationTrainingCoursesCommandHandler handler)
    {
        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<FindAnApprenticeship.Domain.Models.Application>());

        candidateApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(candidateApiResponse), HttpStatusCode.OK, string.Empty));

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Application.Should().NotBeNull();
        result.Application.Should().BeEquivalentTo(candidateApiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
        PatchApplicationTrainingCoursesCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ILogger<PatchApplicationTrainingCoursesCommandHandler>> loggerMock,
        PatchApplicationTrainingCoursesCommandHandler handler)
    {
        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<FindAnApprenticeship.Domain.Models.Application>());

        candidateApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, string.Empty));

        Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };
        await act.Should().ThrowAsync<HttpRequestContentException>();
    }
}
