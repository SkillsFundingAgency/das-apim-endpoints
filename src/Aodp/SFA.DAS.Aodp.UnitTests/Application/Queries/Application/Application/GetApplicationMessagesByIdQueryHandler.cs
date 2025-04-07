using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Aodp.Application.Queries.Application.Application;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Application
{
    [TestFixture]
    public class GetApplicationMessagesByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationMessagesByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationMessagesByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_ApplicationFormData_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationMessagesByIdQuery>();
            var response = new GetApplicationMessagesByIdQueryResponse()
            {

            };

            _apiClientMock.Setup(x => x.Get<GetApplicationMessagesByIdQueryResponse>(It.IsAny<GetApplicationMessagesByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetApplicationMessagesByIdQueryResponse>(It.IsAny<GetApplicationMessagesByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetApplicationMessagesByIdQueryResponse>(It.Is<GetApplicationMessagesByIdApiRequest>(r => r.ApplicationId == query.ApplicationId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationMessagesByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetApplicationMessagesByIdQueryResponse>(It.IsAny<GetApplicationMessagesByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
