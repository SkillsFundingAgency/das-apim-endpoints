using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;


namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingDeleteCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Job_Is_Deleted(
            DeleteJobCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            DeleteJobCommandHandler handler)
        {
            var expectedRequest = new DeleteJobRequest(command.ApplicationId, command.CandidateId, command.JobId);

            candidateApiClient.Setup(client => client.Delete(It.Is<DeleteJobRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)));

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient.Verify(x => x.Delete(It.IsAny<DeleteJobRequest>()), Times.Once);
        }
    }
}
