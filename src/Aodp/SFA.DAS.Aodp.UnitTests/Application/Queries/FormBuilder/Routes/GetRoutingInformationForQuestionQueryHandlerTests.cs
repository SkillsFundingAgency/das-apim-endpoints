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
    public class GetRoutingInformationForQuestionQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetRoutingInformationForQuestionQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetRoutingInformationForQuestionQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Routing_Information_For_Question_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRoutingInformationForQuestionQuery>();
            var response = _fixture.Create<GetRoutingInformationForQuestionQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetRoutingInformationForQuestionQueryResponse>(It.IsAny<GetRoutingInformationForQuestionApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetRoutingInformationForQuestionQueryResponse>(It.IsAny<GetRoutingInformationForQuestionApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetRoutingInformationForQuestionQueryResponse>(It.Is<GetRoutingInformationForQuestionApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetRoutingInformationForQuestionQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetRoutingInformationForQuestionQueryResponse>(It.IsAny<GetRoutingInformationForQuestionApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
