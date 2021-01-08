using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.LegalEntity
{
    public class WhenRefreshingLegalEntitiesVrfStatus
    {
        [Test, MoqAutoData]
        public async Task Then_RefreshVendorRegistrationFormCaseStatusCommand_Is_Sent(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<RefreshVendorRegistrationFormCaseStatusCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.RefreshVendorRegistrationFormStatus() as OkResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}