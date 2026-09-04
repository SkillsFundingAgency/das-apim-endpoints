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

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Files
{
    [TestFixture]
    public class CreateFileMetadataCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private CreateFileMetadataCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();

            _handler = new CreateFileMetadataCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var command = _fixture.Create<CreateFileMetadataCommand>();

            var apiResponse = new ApiResponse<EmptyResponse>(
                new EmptyResponse(),
                System.Net.HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<EmptyResponse>(
                    It.IsAny<CreateFileMetadataApiRequest>()))
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
                x => x.PostWithResponseCode<EmptyResponse>(
                    It.Is<CreateFileMetadataApiRequest>(r => r.Data == command)),
                Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFailure_WhenApiCallThrowsException()
        {
            // Arrange
            var command = _fixture.Create<CreateFileMetadataCommand>();
            var exceptionMessage = "API failure";

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<EmptyResponse>(
                    It.IsAny<CreateFileMetadataApiRequest>()))
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
                x => x.PostWithResponseCode<EmptyResponse>(
                    It.IsAny<CreateFileMetadataApiRequest>()),
                Times.Once);
        }
    }
}