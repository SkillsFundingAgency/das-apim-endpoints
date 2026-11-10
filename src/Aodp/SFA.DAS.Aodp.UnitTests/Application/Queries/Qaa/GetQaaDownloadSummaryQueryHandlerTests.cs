using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Qaa;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qaa;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qaa
{
    [TestFixture]
    public class GetQaaDownloadSummaryQueryHandlerTests
    {
        private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
        private GetQaaDownloadSummaryQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _handler = new GetQaaDownloadSummaryQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_WhenApiReturnsWrappedResponse_ShouldReturnSuccess()
        {
            // Arrange
            var innerResponse = new GetQaaDownloadSummaryQueryResponse
            {
                DataLastImportedDate = new DateTime(2026, 1, 1),
                MostRecentDownloadDate = new DateTime(2026, 1, 2),
                NewQualificationsCount = 1,
                ExtendedQualificationsCount = 2,
                DiscontinuedQualificationsCount = 3,
                DownloadHistory = new List<GetQaaDownloadSummaryQueryResponse.QaaDownloadLog>
                {
                    new() { UserDisplayName = "user1", DownloadDate = new DateTime(2026, 1, 2) }
                }
            };

            _mockApiClient
                .Setup(x => x.Get<GetQaaDownloadSummaryQueryResponse>(
                    It.IsAny<GetQaaDownloadSummaryApiRequest>()))
                .ReturnsAsync(innerResponse);

            var query = new GetQaaDownloadSummaryQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.NewQualificationsCount, Is.EqualTo(1));
            Assert.That(result.Value.ExtendedQualificationsCount, Is.EqualTo(2));
            Assert.That(result.Value.DiscontinuedQualificationsCount, Is.EqualTo(3));
            Assert.That(result.Value.DownloadHistory, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task Handle_WhenApiThrowsException_ShouldReturnFailure()
        {
            // Arrange
            var exceptionMessage = "boom";

            _mockApiClient
                .Setup(x => x.Get<GetQaaDownloadSummaryQueryResponse>(
                    It.IsAny<GetQaaDownloadSummaryApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            var query = new GetQaaDownloadSummaryQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
