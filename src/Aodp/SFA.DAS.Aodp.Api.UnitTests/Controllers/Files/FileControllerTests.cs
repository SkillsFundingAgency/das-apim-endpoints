using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Api.Controllers.Files;
using SFA.DAS.Aodp.Application.Commands.Files;
using SFA.DAS.Aodp.Application.Queries.Files;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Files
{
    [TestFixture]
    public class FilesControllerTests
    {
        private IFixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<FilesController>> _loggerMock;
        private FilesController _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<FilesController>>>();

            _controller = new FilesController(
                _mediatorMock.Object,
                _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        // --------------------
        // CreateFile
        // --------------------

        [Test]
        public async Task CreateFile_ReturnsOkResult()
        {
            // Arrange
            var command = _fixture.Create<CreateFileMetadataCommand>();
            var response = new EmptyResponse();

            var wrapper = new BaseMediatrResponse<EmptyResponse>
            {
                Success = true,
                Value = response
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateFileMetadataCommand>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.CreateFile(command);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(command, default),
                Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task CreateFile_WhenMediatorFails_ReturnsServerError()
        {
            // Arrange
            var command = _fixture.Create<CreateFileMetadataCommand>();

            var wrapper = new BaseMediatrResponse<EmptyResponse>
            {
                Success = false,
                ErrorMessage = "Failure"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateFileMetadataCommand>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.CreateFile(command);

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            Assert.That(((StatusCodeResult)result).StatusCode,
                Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        // --------------------
        // DeleteFile
        // --------------------

        [Test]
        public async Task DeleteFile_ReturnsOkResult()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var response = new EmptyResponse();

            var wrapper = new BaseMediatrResponse<EmptyResponse>
            {
                Success = true,
                Value = response
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteFileMetadataCommand>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.DeleteFile(fileId);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(
                    It.Is<DeleteFileMetadataCommand>(c => c.FileId == fileId),
                    default),
                Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(((OkObjectResult)result).Value, Is.EqualTo(response));
        }

        [Test]
        public async Task DeleteFile_WhenMediatorFails_ReturnsServerError()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            var wrapper = new BaseMediatrResponse<EmptyResponse>
            {
                Success = false,
                ErrorMessage = "Failure"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteFileMetadataCommand>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.DeleteFile(fileId);

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            Assert.That(((StatusCodeResult)result).StatusCode,
                Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        // --------------------
        // GetFiles
        // --------------------

        [Test]
        public async Task GetFiles_ReturnsOkResult()
        {
            // Arrange
            var query = _fixture.Create<GetFileMetadataQuery>();
            var response = _fixture.Create<GetFileMetadataQueryResponse>();

            var wrapper = new BaseMediatrResponse<GetFileMetadataQueryResponse>
            {
                Success = true,
                Value = response
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetFileMetadataQuery>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.GetFiles(query);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(query, default),
                Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(((OkObjectResult)result).Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetFiles_WhenMediatorFails_ReturnsServerError()
        {
            // Arrange
            var query = _fixture.Create<GetFileMetadataQuery>();

            var wrapper = new BaseMediatrResponse<GetFileMetadataQueryResponse>
            {
                Success = false,
                ErrorMessage = "Failure"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetFileMetadataQuery>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.GetFiles(query);

            // Assert
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            Assert.That(((StatusCodeResult)result).StatusCode,
                Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}