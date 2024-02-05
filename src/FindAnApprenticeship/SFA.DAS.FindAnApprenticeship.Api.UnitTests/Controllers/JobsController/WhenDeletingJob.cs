using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.JobsController
{
    [TestFixture]
    public class WhenDeletingWorkHistory
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid workHistoryId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<DeleteJobCommand>(c =>
                    c.CandidateId == candidateId
                    && c.ApplicationId == applicationId
                    && c.JobId == workHistoryId),
                CancellationToken.None));

            var actual = await controller.DeleteWorkHistory(candidateId, applicationId, workHistoryId);

            actual.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid workHistoryId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.JobsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<DeleteJobCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.DeleteWorkHistory(candidateId, applicationId, workHistoryId) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
