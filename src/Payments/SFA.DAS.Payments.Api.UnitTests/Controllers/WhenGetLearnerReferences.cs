using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Payments.Api.Controllers;
using SFA.DAS.Payments.Api.Models;
using SFA.DAS.Payments.Application.Learners;
using SFA.DAS.Payments.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Api.UnitTests.Controllers
{
    [TestFixture]
    internal class WhenGetLearnerReferences
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. - nUnit initializes fields in SetUp
        private Fixture _fixture;
        private Mock<ILogger<IlrController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private IlrController _controller;
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<IlrController>>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new IlrController(_loggerMock.Object, _mediatorMock.Object);
        }

        [Test]
        public async Task Then_ReturnsOkResult_WithLearnerReferences()
        {
            // Arrange
            var ukprn = _fixture.Create<string>();
            var academicYear = _fixture.Create<short>();
            var learnersQueryResult = _fixture.Create<IEnumerable<LearnerResponse>>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLearnersQuery>(), default)).ReturnsAsync(learnersQueryResult);

            // Act
            var result = await _controller.GetLearnerReferences(ukprn, academicYear);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(learnersQueryResult.ToLearnerReferenceResponse());
        }

        [Test]
        public async Task Then_ReturnsBadRequest_OnException()
        {
            // Arrange
            var ukprn = _fixture.Create<string>();
            var academicYear = _fixture.Create<short>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLearnersQuery>(), default))
                         .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetLearnerReferences(ukprn, academicYear);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
