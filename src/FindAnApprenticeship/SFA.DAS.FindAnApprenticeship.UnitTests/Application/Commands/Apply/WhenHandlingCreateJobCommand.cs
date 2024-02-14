using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    [TestFixture]
    public class WhenHandlingCreateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            CreateJobCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            CreateJobCommandHandler handler)
        {
            var expectedRequest = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData());
            candidateApiClient
                .Setup(client => client.Put(
                    It.Is<PutUpsertWorkHistoryApiRequest>(r => r.PutUrl == expectedRequest.PutUrl)))
                .Returns(Task.FromResult(Unit.Value));

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Put(It.IsAny<PutUpsertWorkHistoryApiRequest>()), Times.Once);
        }
    }
}
