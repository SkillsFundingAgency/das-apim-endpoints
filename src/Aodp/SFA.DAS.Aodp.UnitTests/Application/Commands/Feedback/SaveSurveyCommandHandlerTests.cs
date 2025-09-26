using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Feedback;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Feedback
{
    [TestFixture]
    public class SaveSurveyCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private SaveSurveyCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new SaveSurveyCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccessResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var command = _fixture.Create<SaveSurveyCommand>();

         _apiClientMock.Setup(x => x.PostWithResponseCode<EmptyResponse>(It.IsAny<SaveSurveyApiRequest>(),It.IsAny<bool>()))
                          .ReturnsAsync(new ApiResponse<EmptyResponse>(new EmptyResponse(), System.Net.HttpStatusCode.OK, ""));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
        }

        [Test]
        public async Task Handle_ReturnsErrorResponse_WhenApiCallFails()
        {
            // Arrange
            var command = _fixture.Create<SaveSurveyCommand>();
            var exceptionMessage = "API call failed";

            _apiClientMock.Setup(x => x.PostWithResponseCode<EmptyResponse>(It.IsAny<SaveSurveyApiRequest>(), It.IsAny<bool>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
