using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    public class WhenCreatingAnApplication
    {
        [Test, MoqAutoData]
        public async Task Then_CreateApplicationCommand_Is_Sent(
            CreateApplicationRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            var applicationId = request.ApplicationId;

            mockMediator
                .Setup(mediator => mediator.Send(
                        It.Is<CreateApplicationCommand>(c =>
                            c.AccountId == request.AccountId
                              && c.AccountLegalEntityId == request.AccountLegalEntityId
                              && c.ApplicationId == request.ApplicationId
                            && c.ApprenticeshipIds == request.ApprenticeshipIds
                    ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.CreateApplication(request) as CreatedResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Location.Should().Be($"/accounts/{request.AccountId}/applications/{applicationId}");
        }
    }
}