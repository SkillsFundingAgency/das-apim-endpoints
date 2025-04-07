using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Application.Section;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Section
{
    [TestFixture]
    public class GetApplicationSectionStatusByApplicationIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationSectionStatusByApplicationIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationSectionStatusByApplicationIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Section_Status_By_ApplicationId_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationSectionStatusByApplicationIdQuery>();
            var response = _fixture.Create<GetApplicationSectionStatusByApplicationIdQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetApplicationSectionStatusByApplicationIdQueryResponse>(It.IsAny<GetApplicationSectionStatusByIdApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetApplicationSectionStatusByApplicationIdQueryResponse>(It.IsAny<GetApplicationSectionStatusByIdApiRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetApplicationSectionStatusByApplicationIdQueryResponse>(It.Is<GetApplicationSectionStatusByIdApiRequest>(r => r.FormVersionId == query.FormVersionId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationSectionStatusByApplicationIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetApplicationSectionStatusByApplicationIdQueryResponse>(It.IsAny<GetApplicationSectionStatusByIdApiRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
