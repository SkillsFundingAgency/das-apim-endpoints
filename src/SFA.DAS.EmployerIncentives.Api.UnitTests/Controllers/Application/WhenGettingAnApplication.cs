using System;
using System.Net;
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
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EligibleApprenticeshipSearch
{
    public class WhenGettingAnApplication
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Application_From_Mediator(
            long accountId,
            Guid applicationId,
            GetApplicationResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationQuery>(c=>
                                            c.AccountId.Equals(accountId) 
                                            && c.ApplicationId.Equals(applicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApplication(accountId, applicationId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as ApplicationResponse;
            Assert.IsNotNull(model);
            model.Application.Should().BeEquivalentTo(mediatorResult.Application);
        }
    }
}