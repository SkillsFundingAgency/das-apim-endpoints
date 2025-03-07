using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Page
{
    [TestFixture]
    public class GetApplicationPageByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationPageByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationPageByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_PageId_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationPageByIdQuery>();
            var response = new GetApplicationPageByIdQueryResponse()
            {
                Id = Guid.NewGuid(),
                Order = 1,
                TotalSectionPages = 1,
                Questions = {}
            };

            _apiClientMock.Setup(x => x.Get<GetApplicationPageByIdQueryResponse>(It.IsAny<GetApplicationPageByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetApplicationPageByIdQueryResponse>(It.IsAny<GetApplicationPageByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetApplicationPageByIdQueryResponse>(It.Is<GetApplicationPageByIdApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationPageByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetApplicationPageByIdQueryResponse>(It.IsAny<GetApplicationPageByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
