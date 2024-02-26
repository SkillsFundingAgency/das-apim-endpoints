using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    [TestFixture]
    public class WhenHandlingUpdateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_WorkHistoryItem_Is_Updated(
            UpdateJobCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            UpdateJobCommandHandler handler)
        {
            var expectedRequest = new PutUpdateWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, command.JobId, new PutUpdateWorkHistoryApiRequest.PutUpdateWorkHistoryApiRequestData());

            candidateApiClient
                .Setup(client => client.Put(
                    It.Is<PutUpdateWorkHistoryApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                .Returns(Task.FromResult(Unit.Value));

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Put(It.IsAny<PutUpdateWorkHistoryApiRequest>()), Times.Once);
        }
    }
}
