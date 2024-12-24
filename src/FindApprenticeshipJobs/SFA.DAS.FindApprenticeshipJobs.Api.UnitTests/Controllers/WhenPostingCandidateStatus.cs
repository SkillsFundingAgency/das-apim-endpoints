using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenPostingCandidateStatus
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Post_Response(
            string govIdentifier,
            CandidateUpdateStatusRequest model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.CandidatesController controller)
        {
            var actual = await controller.UpdateStatus(govIdentifier, model);

            using (new AssertionScope())
            {
                actual.Should().BeOfType<NoContentResult>();
            }
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string govIdentifier,
            CandidateUpdateStatusRequest model,
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
