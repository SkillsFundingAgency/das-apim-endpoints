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
using SFA.DAS.EmployerIncentives.Application.Commands.RecalculateEarnings;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.ApprenticeshipIncentive
{
    [TestFixture]
    public class WhenRecalculatingEarnings
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Earnings_Recalculations_to_Mediator(
            RecalculateEarningsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticeshipIncentiveController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RecalculateEarningsCommand>(c =>
                        c.RecalculateEarningsRequest.Equals(request)),
                    It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.RecalculateEarnings(request) as NoContentResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        }
    }
}
