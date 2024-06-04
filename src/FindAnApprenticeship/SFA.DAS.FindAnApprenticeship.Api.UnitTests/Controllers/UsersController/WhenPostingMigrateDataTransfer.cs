using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateData;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    [TestFixture]
    public class WhenPostingMigrateDataTransfer
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            PostMigrateDataTransferApiRequest request,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateDataCommand>(q =>
                        q.EmailAddress == request.EmailAddress &&
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var actual = await controller.MigrateDataTransfer(candidateId, request);

            actual.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
            PostMigrateDataTransferApiRequest request,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateDataCommand>(q =>
                        q.EmailAddress == request.EmailAddress &&
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.MigrateDataTransfer(candidateId, request);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
