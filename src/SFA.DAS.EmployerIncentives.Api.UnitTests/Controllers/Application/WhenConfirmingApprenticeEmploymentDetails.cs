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
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmEmploymentDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EmploymentDetails;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    [TestFixture]
    public class WhenConfirmingApprenticeEmploymentDetails
    {
        [Test, MoqAutoData]
        public async Task Then_ConfirmEmploymentDetailsCommand_is_handled(
            long accountId,
            Guid applicationId,
            ConfirmEmploymentDetailsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<ConfirmEmploymentDetailsCommand>(c => 
                     c.ConfirmEmploymentDetailsRequest.AccountId.Equals(request.AccountId) 
                     && c.ConfirmEmploymentDetailsRequest.ApplicationId.Equals(request.ApplicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.ConfirmEmploymentDetails(accountId, applicationId, request) as StatusCodeResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
