using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingUpdateAdditionalQuestionCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_AdditionalQuestion_Is_Updated(
        UpdateAdditionalQuestionCommand command,
        PutUpsertAdditionalQuestionApiResponse additionalQuestionApiResponse,
        Models.Application updateApplicationResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateAdditionalQuestionCommandHandler handler)
    {
        var expectedPostAdditionalQuestionRequest = new PutUpsertAdditionalQuestionApiRequest(command.ApplicationId, command.CandidateId, command.Id, new PutUpsertAdditionalQuestionApiRequest.PutUpsertAdditionalQuestionApiRequestData());

        candidateApiClient
                    .Setup(client => client.PutWithResponseCode<PutUpsertAdditionalQuestionApiResponse>(
                        It.Is<PutUpsertAdditionalQuestionApiRequest>(r => r.PutUrl == expectedPostAdditionalQuestionRequest.PutUrl)))
                    .ReturnsAsync(new ApiResponse<PutUpsertAdditionalQuestionApiResponse>(additionalQuestionApiResponse, HttpStatusCode.OK, string.Empty));

        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<Models.Application>());

        candidateApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(updateApplicationResponse), HttpStatusCode.OK, string.Empty));

        var actual = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Application.Should().BeEquivalentTo(updateApplicationResponse);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Update_Application_Status_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
        UpdateAdditionalQuestionCommand command,
        PutUpsertAdditionalQuestionApiResponse additionalQuestionApiResponse,
        Models.Application updateApplicationResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateAdditionalQuestionCommandHandler handler)
    {
        var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<Models.Application>());

        candidateApiClient
            .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, string.Empty));

        Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

        await act.Should().ThrowAsync<HttpRequestContentException>();
    }
}
