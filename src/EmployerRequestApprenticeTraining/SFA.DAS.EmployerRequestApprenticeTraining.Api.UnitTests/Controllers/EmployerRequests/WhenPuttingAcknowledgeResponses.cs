using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPuttingAcknowledgeResponses
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<EmployerRequestsController>> _loggerMock;
        private EmployerRequestsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<EmployerRequestsController>>();
            _controller = new EmployerRequestsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test, AutoData]
        public async Task Then_Returns_OkResult_When_Command_Is_Successful(Guid employerRequestId, Guid acknowledgedBy)
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AcknowledgeProviderResponsesCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _controller.AcknowledgeProviderResponses(employerRequestId, acknowledgedBy);

            // Assert
            result.Should().BeOfType<OkResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<AcknowledgeProviderResponsesCommand>(c =>
                c.EmployerRequestId == employerRequestId && c.AcknowledgedBy == acknowledgedBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, AutoData]
        public async Task Then_Returns_InternalServerError_When_Exception_Is_Thrown(Guid employerRequestId, Guid acknowledgedBy)
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AcknowledgeProviderResponsesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.AcknowledgeProviderResponses(employerRequestId, acknowledgedBy) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
