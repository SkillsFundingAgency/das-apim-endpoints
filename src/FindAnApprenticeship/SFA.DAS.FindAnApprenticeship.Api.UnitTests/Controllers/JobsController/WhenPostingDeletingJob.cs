﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenDeletingWorkHistory
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid jobId,
            Guid applicationId,
            PostDeleteJobRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<PostDeleteJobCommand>(c =>
                    c.CandidateId == request.CandidateId
                    && c.ApplicationId == applicationId
                    && c.JobId == jobId),
                CancellationToken.None)).ReturnsAsync(new Unit());

            var actual = await controller.PostDeleteJob(applicationId, jobId, request);

            actual.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid jobId,
            Guid applicationId,
            PostDeleteJobRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<PostDeleteJobCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.PostDeleteJob(applicationId, jobId, request) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
