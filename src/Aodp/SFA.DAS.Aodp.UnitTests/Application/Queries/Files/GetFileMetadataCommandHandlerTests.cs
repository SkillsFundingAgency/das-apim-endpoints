using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Files;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Files;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Files
{
    [TestFixture]
    public class GetFileMetadataQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetFileMetadataQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();

            _handler = new GetFileMetadataQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var query = _fixture.Create<GetFileMetadataQuery>();
            var responseBody = _fixture.Create<GetFileMetadataQueryResponse>();

            var apiResponse = new ApiResponse<GetFileMetadataQueryResponse>(
                responseBody,
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<GetFileMetadataQueryResponse>(
                    It.IsAny<GetFileMetadataApiRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.EqualTo(responseBody));
                Assert.That(result.ErrorMessage, Is.Null);
            });

            _apiClientMock.Verify(
                x => x.PostWithResponseCode<GetFileMetadataQueryResponse>(
                    It.Is<GetFileMetadataApiRequest>(r => r.Data == query)),
                Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsFailure_WhenApiCallThrowsException()
        {
            // Arrange
            var query = _fixture.Create<GetFileMetadataQuery>();
            var exceptionMessage = "API failure";

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<GetFileMetadataQueryResponse>(
                    It.IsAny<GetFileMetadataApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
            });

            _apiClientMock.Verify(
                x => x.PostWithResponseCode<GetFileMetadataQueryResponse>(
                    It.IsAny<GetFileMetadataApiRequest>()),
                Times.Once);
        }
    }
}