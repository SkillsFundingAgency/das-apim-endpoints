using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.LegalEntity
{
    public class WhenUpdatingLegalEntityVrfDetails
    {
        [Test, MoqAutoData]
        public async Task Then_UpdateApplicationCommand_Is_Sent(
            long legalEntityId,
            string hashedLegalEntityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpdateVendorRegistrationFormCaseDetailsCommand>(c =>
                        c.LegalEntityId == legalEntityId
                        && c.HashedLegalEntityId == hashedLegalEntityId
                    ), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.UpdateVendorRegistrationForm(legalEntityId, hashedLegalEntityId) as NoContentResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}