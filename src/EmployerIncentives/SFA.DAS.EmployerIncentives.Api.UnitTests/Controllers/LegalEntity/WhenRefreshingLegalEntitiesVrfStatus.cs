using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.LegalEntity
{
    public class WhenRefreshingLegalEntitiesVrfStatus
    {
        [Test, MoqAutoData]
        public async Task Then_RefreshVendorRegistrationFormCaseStatusCommand_Is_Sent_And_Command_output_is_returned(
            DateTime from,
            DateTime lastCaseUpdated,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RefreshVendorRegistrationFormCaseStatusCommand>(x =>
                        x.FromDateTime == from),
                    It.IsAny<CancellationToken>())).ReturnsAsync(lastCaseUpdated);

            var controllerResult = await controller.RefreshVendorRegistrationFormStatus(from) as OkObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.Value.Should().Be(lastCaseUpdated);
        }
    }
}