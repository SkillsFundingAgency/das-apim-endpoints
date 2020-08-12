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
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationLegalEntity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EligibleApprenticeshipSearch
{
    public class WhenGettingLegalEntityForApplication
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Legal_Entity_From_Mediator(
            long accountId,
            Guid applicationId,
            long mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationLegalEntityQuery>(c=>
                                            c.AccountId.Equals(accountId) 
                                            && c.ApplicationId.Equals(applicationId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApplicationLegalEntity(accountId, applicationId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            controllerResult.Value.Should().Be(mediatorResult);
        }
    }
}