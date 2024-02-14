using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingCreateTrainingCourseCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_TrainingCourse_Is_Updated(
        CreateTrainingCourseCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        CreateTrainingCourseCommandHandler handler)
    {
        var expectedRequest = new PutUpsertTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData());

        candidateApiClient
            .Setup(client => client.Put(
                It.Is<PutUpsertTrainingCourseApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
            .Returns(Task.FromResult(Unit.Value));

        await handler.Handle(command, CancellationToken.None);

        candidateApiClient.Verify(x => x.Put(It.IsAny<PutUpsertTrainingCourseApiRequest>()), Times.Once);
    }
}
