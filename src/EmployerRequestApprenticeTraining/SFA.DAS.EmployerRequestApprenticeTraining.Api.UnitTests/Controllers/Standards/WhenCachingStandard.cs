using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CacheStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.Standards
{
    public class WhenCachingStandard
    {
        private Mock<IMediator> _mockMediator;
        private Mock<ILogger<StandardsController>> _mockLogger;
        private StandardsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<StandardsController>>();
            _sut = new StandardsController(_mockMediator.Object, _mockLogger.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Standard_Is_Returned(
            CacheStandardResult result)
        {
            // Arrange
            _mockMediator
                .Setup(x => x.Send(It.IsAny<CacheStandardCommand>(), CancellationToken.None))
                .ReturnsAsync(result);
            // Act
            var actual = await _sut.Cache("1001") as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<Standard>().Which.Should().BeEquivalentTo(result.Standard);
        }

        [Test, MoqAutoData]
        public async Task CacheStandardCommand_IsUnsuccessful_Then_ReturnBadRequest
            (SendResponseNotificationEmailParameters param)
        {
            // Arrange
            _mockMediator
               .Setup(m => m.Send(It.IsAny<CacheStandardCommand>(), It.IsAny<CancellationToken>()))
               .Throws(new Exception());

            // Act
            var result = await _sut.Cache("1001") as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        }
    }
}
