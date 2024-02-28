using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingUpdateAdditionalQuestionCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_AdditionalQuestion_Is_Updated(
        UpdateAdditionalQuestionCommand command,
        PutUpsertAdditionalQuestionApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateAdditionalQuestionCommandHandler handler)
    {
        var expectedRequest = new PutUpsertAdditionalQuestionApiRequest(command.ApplicationId, command.CandidateId, new PutUpsertAdditionalQuestionApiRequest.PutUpsertAdditionalQuestionApiRequestData());

        candidateApiClient
                    .Setup(client => client.PutWithResponseCode<PutUpsertAdditionalQuestionApiResponse>(
                        It.Is<PutUpsertAdditionalQuestionApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                    .ReturnsAsync(new ApiResponse<PutUpsertAdditionalQuestionApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        var actual = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
        }
    }
}
