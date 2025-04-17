using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Application.Qualifications
{
    [TestFixture]
    public class CreateQualificationDiscussionHistoryNoteForFundingOffersCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private CreateQualificationDiscussionHistoryNoteForFundingOffersCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new CreateQualificationDiscussionHistoryNoteForFundingOffersCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccessResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var command = _fixture.Create<CreateQualificationDiscussionHistoryNoteForFundingOffersCommand>();

            _apiClientMock.Setup(x => x.Put(It.IsAny<CreateQualificationDiscussionHistoryNoteForFundingApiRequest>()))
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
            var command = _fixture.Create<CreateQualificationDiscussionHistoryNoteForFundingOffersCommand>();
            var exceptionMessage = "API call failed";

            _apiClientMock.Setup(x => x.Put(It.IsAny<CreateQualificationDiscussionHistoryNoteForFundingApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
