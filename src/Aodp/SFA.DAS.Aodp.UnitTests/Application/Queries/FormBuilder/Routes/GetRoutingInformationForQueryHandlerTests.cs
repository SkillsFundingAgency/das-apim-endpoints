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
    public class GetRoutingInformationForQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetRoutingInformationForFormQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetRoutingInformationForFormQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Routing_Information_For_Form_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRoutingInformationForFormQuery>();
            var response = new GetRoutingInformationForFormQueryResponse()
            {
                Sections = { },
                Editable = true
            };

            _apiClientMock.Setup(x => x.Get<GetRoutingInformationForFormQueryResponse>(It.IsAny<GetRoutesForFormVersionApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetRoutingInformationForFormQueryResponse>(It.IsAny<GetRoutesForFormVersionApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetRoutingInformationForFormQueryResponse>(It.Is<GetRoutesForFormVersionApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRoutingInformationForFormQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetRoutingInformationForFormQueryResponse>(It.IsAny<GetRoutesForFormVersionApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
