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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenPostingJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        PostJobApiRequest apiRequest,
        CreateJobCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<CreateJobCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.EmployerName == apiRequest.EmployerName
                        && c.JobDescription == apiRequest.JobDescription
                        && c.JobTitle == apiRequest.JobTitle
                        && c.StartDate == apiRequest.StartDate
                        && c.EndDate == apiRequest.EndDate),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.PostJob(applicationId, apiRequest) as CreatedResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as PostJobApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((PostJobApiResponse)result);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid applicationId,
            PostJobApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateJobCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.PostJob(applicationId, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Null_Is_Returned_Then_Not_Found_Response_Returned(
            Guid applicationId,
            PostJobApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateJobCommand>(),
                    CancellationToken.None))
                .ReturnsAsync((() => null));

            var actual = await controller.PostJob(applicationId, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
