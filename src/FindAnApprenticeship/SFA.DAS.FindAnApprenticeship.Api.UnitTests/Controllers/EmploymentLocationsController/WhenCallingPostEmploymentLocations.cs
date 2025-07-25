﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.EmploymentLocationsController
{
    [TestFixture]
    public class WhenCallingPostEmploymentLocations
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid applicationId,
            UpdateEmploymentLocationsCommandResult commandResult,
            PostEmploymentLocationsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateEmploymentLocationsCommand>(c =>
                c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId),
                CancellationToken.None))
                .ReturnsAsync(commandResult);

            var actual = await controller.Post(applicationId, apiRequest);
            actual.Should().BeOfType<CreatedResult>();
            actual.As<CreatedResult>().Value.Should().BeEquivalentTo(commandResult.Application);
        }

        [Test, MoqAutoData]
        public async Task And_Command_Response_Is_Null_Then_NotFound_Returned(
            Guid applicationId,
            UpdateEmploymentLocationsCommandResult commandResult,
            PostEmploymentLocationsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<UpdateEmploymentLocationsCommand>(c =>
                        c.CandidateId.Equals(apiRequest.CandidateId)
                        && c.ApplicationId == applicationId),
                    CancellationToken.None))
                .ReturnsAsync(() => null);
            var actual = await controller.Post(applicationId, apiRequest);
            actual.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task And_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            Guid applicationId,
            UpdateEmploymentLocationsCommandResult commandResult,
            PostEmploymentLocationsApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EmploymentLocationsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<UpdateEmploymentLocationsCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.Post(applicationId, apiRequest) as StatusCodeResult;

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
