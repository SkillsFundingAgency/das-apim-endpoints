using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Application
{
    [TestFixture]
    public class GetApplicationMetadataByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationMetadataByIdQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetApplicationMetadataByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Metadata_By_Id_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationMetadataByIdQuery>();
            var response = _fixture.Create<GetApplicationMetadataByIdQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetApplicationMetadataByIdQueryResponse>(It.IsAny<GetApplicationMetadataByIdRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetApplicationMetadataByIdQueryResponse>(It.IsAny<GetApplicationMetadataByIdRequest>()), Times.Once);

            _apiClientMock.Verify(x => x.Get<GetApplicationMetadataByIdQueryResponse>(It.Is<GetApplicationMetadataByIdRequest>(r => r.ApplicationId == query.ApplicationId)), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetApplicationMetadataByIdQuery>();
            var exception = _fixture.Create<Exception>();

            _apiClientMock.Setup(x => x.Get<GetApplicationMetadataByIdQueryResponse>(It.IsAny<GetApplicationMetadataByIdRequest>()))
                          .ThrowsAsync(exception);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exception.Message));
        }
    }
}
