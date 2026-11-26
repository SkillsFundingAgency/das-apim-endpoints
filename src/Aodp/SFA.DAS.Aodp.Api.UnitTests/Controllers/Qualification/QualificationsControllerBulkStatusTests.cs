using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Commands.Qualifications;
using SFA.DAS.AODP.Api.Controllers;
using SFA.DAS.AODP.Api.Controllers.Qualification;
using SFA.DAS.AODP.Application.Commands.Qualifications;


namespace SFA.DAS.Aodp.UnitTests.Api.Controllers.Qualifications
{
    [TestFixture]
    public class QualificationsControllerBulkStatusUpdateTests
    {
        private IFixture _fixture;
        private Mock<ILogger<QualificationsController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private QualificationsController _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _loggerMock = _fixture.Freeze<Mock<ILogger<QualificationsController>>>();
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();

            _controller = new QualificationsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task BulkStatusUpdate_ReturnsOk_WhenMediatorReturnsSuccess()
        {
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();

            var mediatorResponse = new BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>
            {
                Success = true,
                Value = new BulkUpdateQualificationStatusCommandResponse()
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var result = await _controller.BulkStatusUpdate(command);

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.EqualTo(mediatorResponse.Value));

            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task BulkStatusUpdate_ReturnsInternalServerError_WhenMediatorReturns404()
        {
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();

            var mediatorResponse = new BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>
            {
                Success = false,
                ErrorMessage = "Not found"
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var result = await _controller.BulkStatusUpdate(command);

            var status = result as StatusCodeResult;
            Assert.That(status, Is.Not.Null);
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task BulkStatusUpdate_ReturnsInternalServerError_WhenMediatorReturns500()
        {
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();

            var mediatorResponse = new BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse>
            {
                Success = false,
                ErrorMessage = "Server error"
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var result = await _controller.BulkStatusUpdate(command);

            var status = result as StatusCodeResult;
            Assert.That(status, Is.Not.Null);
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }


        [Test]
        public async Task BulkStatusUpdate_CallsMediatorWithCorrectCommand()
        {
            var command = _fixture.Create<BulkUpdateQualificationStatusCommand>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<BulkUpdateQualificationStatusCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BaseMediatrResponse<BulkUpdateQualificationStatusCommandResponse> { Success = true });

            await _controller.BulkStatusUpdate(command);

            _mediatorMock.Verify(m =>
                m.Send(
                    It.Is<BulkUpdateQualificationStatusCommand>(c =>
                        c.ProcessStatusId == command.ProcessStatusId &&
                        c.Comment == command.Comment &&
                        c.UserDisplayName == command.UserDisplayName &&
                        c.QualificationIds.SequenceEqual(command.QualificationIds)
                    ),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
