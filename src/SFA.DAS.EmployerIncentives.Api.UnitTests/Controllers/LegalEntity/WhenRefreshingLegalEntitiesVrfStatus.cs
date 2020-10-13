using AutoFixture.NUnit3;
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
        public async Task Then_RefreshVendorRegistrationFormCaseStatusCommand_Is_Sent(
            DateTime from,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RefreshVendorRegistrationFormCaseStatusCommand>(x =>
                        x.FromDateTime == from),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.RefreshVendorRegistrationFormStatus(from) as NoContentResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}