using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Commands.Files;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Files;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Files
{
    [TestFixture]
    public class DeleteFileMetadataCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private DeleteFileMetadataCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();

            _handler = new DeleteFileMetadataCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var command = _fixture.Create<DeleteFileMetadataCommand>();

            var apiResponse = new ApiResponse<EmptyResponse>(
                new EmptyResponse(),
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.DeleteWithResponseCode<EmptyResponse>(
                    It.IsAny<DeleteFileMetadataApiRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.ErrorMessage, Is.Null);
            });

            _apiClientMock.Verify(
                x => x.DeleteWithResponseCode<EmptyResponse>(
                    It.Is<DeleteFileMetadataApiRequest>(r =>
                        r.FileId == command.FileId)),
                Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFailure_WhenApiCallThrowsException()
        {
            // Arrange
            var command = _fixture.Create<DeleteFileMetadataCommand>();
            var exceptionMessage = "API delete failed";

            _apiClientMock
                .Setup(x => x.DeleteWithResponseCode<EmptyResponse>(
                    It.IsAny<DeleteFileMetadataApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
            });

            _apiClientMock.Verify(
                x => x.DeleteWithResponseCode<EmptyResponse>(
                    It.IsAny<DeleteFileMetadataApiRequest>()),
                Times.Once);
        }
    }
}