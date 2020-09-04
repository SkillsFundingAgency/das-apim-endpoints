using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.LegalEntity
{
    public class WhenUpdatingLegalEntityVrfStatus
    {
        [Test, MoqAutoData]
        public async Task Then_UpdateVendorRegistrationFormCaseStatusCommand_Is_Sent(
            long legalEntityId,
            string caseId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpdateVendorRegistrationFormCaseStatusCommand>(c =>
                        c.LegalEntityId == legalEntityId
                        && c.CaseId == caseId
                    ), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.UpdateVendorRegistrationFormStatus(legalEntityId, caseId) as NoContentResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}