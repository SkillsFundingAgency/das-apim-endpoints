using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes;
using SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.FormBuilder.Routes
{
    [TestFixture]
    public class GetAvailableSectionsAndPagesForRoutingHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetAvailableSectionsAndPagesForRoutingQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Available_Sections_And_Pages_For_Routing_Are_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQuery>();
            var response = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetAvailableSectionsAndPagesForRoutingQueryResponse>(It.IsAny<GetAvailableSectionsAndPagesForRoutingApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetAvailableSectionsAndPagesForRoutingQueryResponse>(It.IsAny<GetAvailableSectionsAndPagesForRoutingApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetAvailableSectionsAndPagesForRoutingQueryResponse>(It.Is<GetAvailableSectionsAndPagesForRoutingApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetAvailableSectionsAndPagesForRoutingQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetAvailableSectionsAndPagesForRoutingQueryResponse>(It.IsAny<GetAvailableSectionsAndPagesForRoutingApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
