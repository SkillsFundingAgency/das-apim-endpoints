﻿using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PostDeleteTrainingCourse;
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
