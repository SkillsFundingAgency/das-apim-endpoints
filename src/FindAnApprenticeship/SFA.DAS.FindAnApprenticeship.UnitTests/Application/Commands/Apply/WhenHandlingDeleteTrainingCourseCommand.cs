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
    public class WhenHandlingDeleteTrainingCourseCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Job_Is_Deleted(
            PostDeleteTrainingCourseCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            PostDeleteTrainingCourseCommandHandler handler)
        {
            var expectedRequest = new DeleteTrainingCourseRequest(command.ApplicationId, command.CandidateId, command.TrainingCourseId);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            candidateApiClient
                .Verify(client => client.Delete(It.Is<DeleteTrainingCourseRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)), Times.Once);
        }
    }
}
