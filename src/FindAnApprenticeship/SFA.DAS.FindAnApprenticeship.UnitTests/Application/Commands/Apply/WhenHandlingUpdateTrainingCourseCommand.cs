using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;
public class WhenHandlingUpdateTrainingCourseCommand
{
    public class WhenHandlingUpdateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_TrainingCourseItem_Is_Updated(
            UpdateTrainingCourseCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            UpdateTrainingCourseCommandHandler handler)
        {
            var expectedRequest = new PutUpdateTrainingCourseApiRequest(command.ApplicationId, command.CandidateId, command.TrainingCourseId, new PutUpdateTrainingCourseApiRequest.PutUpdateTrainingCourseApiRequestData());

            candidateApiClient
                .Setup(client => client.Put(
                    It.Is<PutUpdateTrainingCourseApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)));

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Put(It.IsAny<PutUpdateTrainingCourseApiRequest>()), Times.Once);
        }
    }
}
