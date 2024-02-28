using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    public class WhenPostingUpdateJob
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid applicationId,
            Guid jobId,
            UpdateJobCommandResult commandResult,
            PostUpdateJobApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateJobCommand>(c =>
                        c.JobId == jobId
                        && c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.Employer == apiRequest.EmployerName
                        && c.Description == apiRequest.JobDescription
                        && c.JobTitle == apiRequest.JobTitle
                        && c.StartDate == apiRequest.StartDate
                        && c.EndDate == apiRequest.EndDate),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            var actual = await controller.PostUpdateJob(applicationId, jobId, apiRequest);

            using (new AssertionScope())
            {
                actual.Should().BeOfType<OkObjectResult>();
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(commandResult.Id);
            }
        }

        [Test, MoqAutoData]
        public async Task And_CommandResult_Is_Null_Then_Returns_NotFound(
            Guid applicationId,
            Guid jobId,
            PostUpdateJobApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateJobCommand>(c =>
                        c.JobId == jobId
                        && c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.Employer == apiRequest.EmployerName
                        && c.Description == apiRequest.JobDescription
                        && c.JobTitle == apiRequest.JobTitle
                        && c.StartDate == apiRequest.StartDate
                        && c.EndDate == apiRequest.EndDate),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var actual = await controller.PostUpdateJob(applicationId, jobId, apiRequest);

            actual.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Thrown_Then_Internal_Server_Error_Returned(
            Guid applicationId,
            Guid jobId,
            PostUpdateJobApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateJobCommand>(c =>
                        c.JobId == jobId
                        && c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId
                        && c.Employer == apiRequest.EmployerName
                        && c.Description == apiRequest.JobDescription
                        && c.JobTitle == apiRequest.JobTitle
                        && c.StartDate == apiRequest.StartDate
                        && c.EndDate == apiRequest.EndDate),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var actual = await controller.PostUpdateJob(applicationId, jobId, apiRequest) as StatusCodeResult;

            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
