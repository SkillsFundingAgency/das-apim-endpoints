using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Status;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.CandidatesController
{
    public class WhenPostingCandidateStatus
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Post_Response(
            string govIdentifier,
            CandidateStatus model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.CandidatesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateCandidateStatusCommand>(x => x.GovUkIdentifier == govIdentifier 
                        && (x.Email == model.Email)
                        && x.Status == model.Status),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var actual = await controller.UpdateStatus(govIdentifier, model);

            using (new AssertionScope())
            {
                actual.Should().BeOfType<OkObjectResult>();
            }
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string govIdentifier,
            CandidateStatus model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.CandidatesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateCandidateStatusCommand>(x => x.GovUkIdentifier == govIdentifier
                        && (x.Email == model.Email)
                        && x.Status == model.Status),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception());

            var actual = await controller.UpdateStatus(govIdentifier, model) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
