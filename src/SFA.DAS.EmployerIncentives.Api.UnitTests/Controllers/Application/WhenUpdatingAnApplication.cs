﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    public class WhenUpdatingAnApplication
    {
        [Test, MoqAutoData]
        public async Task Then_UpdateApplicationCommand_Is_Sent(
            UpdateApplicationRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            var applicationId = request.ApplicationId;

            mockMediator
                .Setup(mediator => mediator.Send(
                        It.Is<UpdateApplicationCommand>(c =>
                            c.AccountId == request.AccountId
                              && c.AccountLegalEntityId == request.AccountLegalEntityId
                              && c.ApplicationId == request.ApplicationId
                            && c.ApprenticeshipIds == request.ApprenticeshipIds
                    ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applicationId);

            var controllerResult = await controller.UpdateApplication(request) as OkObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            controllerResult.Value.Should().Be($"/accounts/{request.AccountId}/applications/{applicationId}");
        }
    }
}