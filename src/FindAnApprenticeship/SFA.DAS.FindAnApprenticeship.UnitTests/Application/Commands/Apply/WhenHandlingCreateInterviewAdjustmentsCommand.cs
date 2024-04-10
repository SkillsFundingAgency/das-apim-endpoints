using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateInterviewAdjustments;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingCreateInterviewAdjustmentsCommand
{
    [Test, MoqAutoData]
    public async Task Then_InterviewAdjustments_Is_Created(
        UpsertInterviewAdjustmentsCommand command,
        Models.Application updateApplicationResponse,
        GetAboutYouItemApiResponse apiResponse,
        PutUpsertAboutYouItemApiResponse createdItemResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ILogger<UpsertInterviewAdjustmentsCommandHandler>> loggerMock,
        UpsertInterviewAdjustmentsCommandHandler handler)
    {
        var expectedRequest = new PutUpsertAboutYouItemApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertAboutYouItemApiRequest.PutUpdateAboutYouItemApiRequestData());

        candidateApiClient
            .Setup(client => client.PutWithResponseCode<PutUpsertAboutYouItemApiResponse>(
                It.Is<PutUpsertAboutYouItemApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
            .ReturnsAsync(new ApiResponse<PutUpsertAboutYouItemApiResponse>(createdItemResponse, HttpStatusCode.OK, string.Empty));

        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<Models.Application>());

        candidateApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(updateApplicationResponse), HttpStatusCode.OK, string.Empty));

        var expectedApiRequest = new GetAboutYouItemApiRequest(command.ApplicationId, command.CandidateId);
        candidateApiClient
            .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                It.Is<GetAboutYouItemApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var actual = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
            actual.Application.Should().BeEquivalentTo(updateApplicationResponse);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Update_Application_Status_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
        UpsertInterviewAdjustmentsCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ILogger<UpsertInterviewAdjustmentsCommandHandler>> loggerMock,
        UpsertInterviewAdjustmentsCommandHandler handler)
    {
        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<Models.Application>());

        candidateApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, string.Empty));

        Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

        await act.Should().ThrowAsync<HttpRequestContentException>();
    }
}
