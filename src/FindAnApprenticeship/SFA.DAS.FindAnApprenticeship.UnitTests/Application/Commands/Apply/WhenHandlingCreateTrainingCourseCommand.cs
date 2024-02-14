using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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
    public async Task Then_The_TrainingCourse_Is_Updated(
        CreateTrainingCourseCommand command,
        PutUpsertTrainingCourseApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        CreateTrainingCourseCommandHandler handler)
    {
        var expectedRequest = new PutUpsertTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData());

        candidateApiClient
            .Setup(client => client.PutWithResponseCode<PutUpsertTrainingCourseApiResponse>(
                It.Is<PutUpsertTrainingCourseApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
            .ReturnsAsync(new ApiResponse<PutUpsertTrainingCourseApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

        var actual = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBeEmpty();
        }
    }
}
