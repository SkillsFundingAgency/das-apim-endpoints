using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.FormBuilder.Tests
{
    [TestFixture]
    public class DeleteRouteCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private DeleteRouteCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<DeleteRouteCommandHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request()
        {
            // Arrange
            var query = _fixture.Create<DeleteRouteCommand>();
            var response = _fixture.Create<EmptyResponse>();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Delete(It.IsAny<DeleteRouteApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Delete(It.Is<DeleteRouteApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);
            _apiClientMock.Verify(x => x.Delete(It.Is<DeleteRouteApiRequest>(r => r.QuestionId == query.QuestionId)), Times.Once);
            _apiClientMock.Verify(x => x.Delete(It.Is<DeleteRouteApiRequest>(r => r.SectionId == query.SectionId)), Times.Once);
            _apiClientMock.Verify(x => x.Delete(It.Is<DeleteRouteApiRequest>(r => r.PageId == query.PageId)), Times.Once);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<DeleteRouteCommand>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Delete(It.IsAny<DeleteRouteApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
