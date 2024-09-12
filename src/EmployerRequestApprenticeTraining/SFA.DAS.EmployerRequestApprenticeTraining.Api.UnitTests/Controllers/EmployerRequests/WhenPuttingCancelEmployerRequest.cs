using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPuttingCancelEmployerRequest
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
        public async Task Then_Returns_OkResult_WhenEmployerRequestIsActive_And_Command_Is_Successful(
            Guid employerRequestId, 
            Guid cancelledBy,
            GetEmployerRequestResult employerRequestResult,
            GetStandardResult standardResult,
            GetEmployerProfileUserResult employerProfileUserResult)
        {
            // Arrange
            employerRequestResult.EmployerRequest.Status = RequestStatus.Active;

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetEmployerRequestQuery>(p => p.EmployerRequestId == employerRequestId), CancellationToken.None))
                .ReturnsAsync(employerRequestResult);

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetStandardQuery>(p => p.StandardId == employerRequestResult.EmployerRequest.StandardReference), CancellationToken.None))
                .ReturnsAsync(standardResult);

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetEmployerProfileUserQuery>(p => p.UserId == cancelledBy), CancellationToken.None))
                .ReturnsAsync(employerProfileUserResult);

            // Act
            var result = await _controller.CancelEmployerRequest(employerRequestId, new Models.CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            result.Should().BeOfType<OkResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<CancelEmployerRequestCommand>(c =>
                c.EmployerRequestId == employerRequestId && c.CancelledBy == cancelledBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqInlineAutoData(RequestStatus.Expired)]
        [MoqInlineAutoData(RequestStatus.Cancelled)]
        public async Task Then_DoesNotCallCancel_WhenEmployerRequestIsNotActive(
            RequestStatus status,
            Guid employerRequestId,
            Guid cancelledBy,
            GetEmployerRequestResult employerRequestResult,
            GetStandardResult standardResult,
            GetEmployerProfileUserResult employerProfileUserResult)
        {
            // Arrange
            employerRequestResult.EmployerRequest.Status = status;

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetEmployerRequestQuery>(p => p.EmployerRequestId == employerRequestId), CancellationToken.None))
                .ReturnsAsync(employerRequestResult);

            // Act
            var result = await _controller.CancelEmployerRequest(employerRequestId, new Models.CancelEmployerRequestRequest { CancelledBy = cancelledBy });

            // Assert
            result.Should().BeOfType<OkResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<CancelEmployerRequestCommand>(c =>
                c.EmployerRequestId == employerRequestId && c.CancelledBy == cancelledBy),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, AutoData]
        public async Task Then_Returns_InternalServerError_When_Exception_Is_Thrown(
            Guid employerRequestId, 
            Guid cancelledBy,
            GetEmployerRequestResult employerRequestResult,
            GetStandardResult standardResult,
            GetEmployerProfileUserResult employerProfileUserResult)
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(It.Is<GetEmployerRequestQuery>(p => p.EmployerRequestId == employerRequestId), CancellationToken.None))
                .ReturnsAsync(employerRequestResult);

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetStandardQuery>(p => p.StandardId == employerRequestResult.EmployerRequest.StandardReference), CancellationToken.None))
                .ReturnsAsync(standardResult);

            _mediatorMock
                .Setup(x => x.Send(It.Is<GetEmployerProfileUserQuery>(p => p.UserId == cancelledBy), CancellationToken.None))
                .ReturnsAsync(employerProfileUserResult);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CancelEmployerRequestCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CancelEmployerRequest(employerRequestId, new Models.CancelEmployerRequestRequest { CancelledBy = cancelledBy }) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
