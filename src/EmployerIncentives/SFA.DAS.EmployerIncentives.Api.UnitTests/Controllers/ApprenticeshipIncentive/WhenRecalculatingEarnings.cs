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
using SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.SharedOuterApi.Infrastructure;
using AutoFixture;

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

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        }

        [Test]
        public async Task Then_a_bad_request_response_is_returned_if_the_inner_api_call_is_not_successful()
        {
            // Arrange
            var fixture = new Fixture();
            var mediator = new Mock<IMediator>();
            var exception = new HttpRequestContentException("API error", HttpStatusCode.BadRequest, "Invalid request");
            mediator.Setup(x => x.Send(It.IsAny<RecalculateEarningsCommand>(), It.IsAny<CancellationToken>())).Throws(exception);
            
            var controller = new ApprenticeshipIncentiveController(mediator.Object);
            var request = fixture.Create<RecalculateEarningsRequest>();

            // Act
            var result = await controller.RecalculateEarnings(request) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be(exception.ErrorContent);
        }
    }
}
