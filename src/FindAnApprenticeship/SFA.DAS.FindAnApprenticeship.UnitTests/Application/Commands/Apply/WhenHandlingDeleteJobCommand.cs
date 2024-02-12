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
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.SharedOuterApi.Infrastructure;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.DeleteJobRequest;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
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
            var expectedRequest = new PostDeleteJobRequest(command.ApplicationId, command.CandidateId, new PostDeleteJobRequestData
            {
                JobId = command.JobId,
            });

            candidateApiClient
                .Setup(client => client.Delete(It.Is<DeleteJobRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)));
                .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
        }
    }
}
