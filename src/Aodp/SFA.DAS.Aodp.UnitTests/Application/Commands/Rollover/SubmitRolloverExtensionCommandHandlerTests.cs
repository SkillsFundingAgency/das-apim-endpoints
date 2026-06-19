using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;
using SFA.DAS.AODP.Application.Commands.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Rollover
{
    [TestFixture]
    public class SubmitRolloverExtensionCommandhandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private SubmitRolloverExtensionCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new SubmitRolloverExtensionCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_WhenApiClientReturnsSuccess_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = _fixture.Create<SubmitRolloverExtensionCommand>();
            var resultMessageText = "the result";

            var apiResponse = new ApiResponse<SubmitRolloverExtensionCommandResponse>(
                new SubmitRolloverExtensionCommandResponse
                {
                    ResultMessage = resultMessageText,
                },
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(c => c.PostWithResponseCode<SubmitRolloverExtensionCommandResponse>(
                    It.IsAny<SubmitRolloverExtensionApiRequest>(), true))
                .ReturnsAsync(apiResponse);

       

            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.ResultMessage, Is.EqualTo(resultMessageText));

            _apiClientMock.Verify(c =>
                c.PostWithResponseCode<SubmitRolloverExtensionCommandResponse>(
                    It.IsAny<SubmitRolloverExtensionApiRequest>(), true),
                Times.Once);
        }

        [Test]
        public async Task Handle_WhenApiClientThrows_ShouldReturnFailureResponse()
        {
            // Arrange
            var command = _fixture.Create<SubmitRolloverExtensionCommand>();

            _apiClientMock
                .Setup(c => c.PostWithResponseCode<SubmitRolloverExtensionCommandResponse>(
                    It.IsAny<SubmitRolloverExtensionApiRequest>(), true))
                .ThrowsAsync(new Exception("API failure"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("API failure"));

            _apiClientMock.Verify(c =>
                c.PostWithResponseCode<SubmitRolloverExtensionCommandResponse>(
                    It.IsAny<SubmitRolloverExtensionApiRequest>(), true),
                Times.Once);
        }

        [Test]
        public async Task Handle_ShouldSendCorrectApiRequest()
        {
            // Arrange
            var command = _fixture.Create<SubmitRolloverExtensionCommand>();

            SubmitRolloverExtensionApiRequest? capturedRequest = null;

            _apiClientMock
                .Setup(c => c.PostWithResponseCode<SubmitRolloverExtensionCommandResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .Callback<IPostApiRequest, bool>((req, _) =>
                {
                    capturedRequest = req as SubmitRolloverExtensionApiRequest;
                })
                .ReturnsAsync(new ApiResponse<SubmitRolloverExtensionCommandResponse>(
                    new SubmitRolloverExtensionCommandResponse(),
                    HttpStatusCode.OK,
                    string.Empty));
            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Data, Is.EqualTo(command));
        }



    }
}
