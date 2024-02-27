using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using MediatR;
using FluentAssertions;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingDeleteJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Job_Is_Deleted(
            PostDeleteJobCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            PostDeleteJobCommandHandler handler)
        {
            var expectedRequest = new PostDeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.JobId);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            candidateApiClient
                .Verify(client => client.Delete(It.Is<PostDeleteJobApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
