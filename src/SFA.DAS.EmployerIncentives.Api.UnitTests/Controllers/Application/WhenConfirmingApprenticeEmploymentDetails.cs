using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.SaveApprenticeshipDetails;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    [TestFixture]
    public class WhenConfirmingApprenticeEmploymentDetails
    {
        [Test, MoqAutoData]
        public async Task Then_ConfirmEmploymentDetailsCommand_is_handled(
            long accountId,
            Guid applicationId,
            ApprenticeshipDetailsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<SaveApprenticeshipDetailsCommand>(c => 
                     c .ApprenticeshipDetailsRequest.AccountId.Equals(request.AccountId) 
                     && c.ApprenticeshipDetailsRequest.ApplicationId.Equals(request.ApplicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.SaveApprenticeshipDetailsDetails(accountId, applicationId, request) as StatusCodeResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
