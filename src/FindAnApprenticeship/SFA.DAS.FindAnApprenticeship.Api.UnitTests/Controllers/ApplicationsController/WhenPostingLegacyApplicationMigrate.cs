using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateLegacyApplications;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationsController
{
    [TestFixture]
    public class WhenPostingLegacyApplicationMigrate
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            PostMigrateLegacyApplicationsRequest request,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateApplicationsCommand>(q =>
                        q.EmailAddress == request.EmailAddress && 
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var actual = await controller.MigrateLegacyApplications(candidateId, request);

            actual.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
            PostMigrateLegacyApplicationsRequest request,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateApplicationsCommand>(q =>
                        q.EmailAddress == request.EmailAddress &&
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.MigrateLegacyApplications(candidateId, request);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
