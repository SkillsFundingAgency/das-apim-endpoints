using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Application.Review
{
    [TestFixture]
    public class SaveQualificationFundingOffersCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private SaveQualificationFundingOffersCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new SaveQualificationFundingOffersCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccessResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var command = _fixture.Create<SaveQualificationFundingOffersCommand>();

            _apiClientMock.Setup(x => x.Put(It.IsAny<SaveQualificationFundingOffersApiRequest>()))
                          .Returns(Task.CompletedTask);

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
            var command = _fixture.Create<SaveQualificationFundingOffersCommand>();
            var exceptionMessage = "API call failed";

            _apiClientMock.Setup(x => x.Put(It.IsAny<SaveQualificationFundingOffersApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}

