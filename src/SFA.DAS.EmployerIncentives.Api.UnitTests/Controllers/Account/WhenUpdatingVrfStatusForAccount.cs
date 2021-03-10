using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    [TestFixture]
    public class WhenUpdatingVrfStatusForAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Updates_Vrf_Case_Status_Calling_Mediator(
            long accountId,
            long accountLegalEntityId,
            string vrfCaseStatus,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            var controllerResult = await controller.UpdateVrfCaseStatus(accountId, accountLegalEntityId, vrfCaseStatus) as NoContentResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<UpdateVendorRegistrationCaseStatusCommand>(c =>
                        c.AccountId.Equals(accountId)
                        && c.AccountLegalEntityId.Equals(accountLegalEntityId)
                        && c.VrfCaseStatus.Equals(vrfCaseStatus)),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
