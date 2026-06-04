using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover
{
    [TestFixture]
    public class GetRolloverCandidatesForExportQueryHandlerTests
    {
        private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
        private GetRolloverCandidatesForExportQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _handler = new GetRolloverCandidatesForExportQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_WhenApiReturnsWrappedResponse_ShouldReturnSuccess()
        {
            // Arrange
            var workflowRunId = Guid.NewGuid();

            var innerResponse = new BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>
            {
                Success = true,
                Value = new GetRolloverCandidatesForExportQueryResponse
                {
                    FileContent = new byte[] { 1, 2, 3 },
                    FileName = "export.csv",
                    ContentType = "text/csv"
                }
            };

            _mockApiClient
                .Setup(x => x.Get<BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>>(
                    It.IsAny<GetRolloverCandidatesForExportApiRequest>()))
                .ReturnsAsync(innerResponse);

            var query = new GetRolloverCandidatesForExportQuery
            {
                RolloverWorkflowRunId = workflowRunId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.FileName, Is.EqualTo("export.csv"));
            Assert.That(result.Value.ContentType, Is.EqualTo("text/csv"));
            Assert.That(result.Value.FileContent, Is.EqualTo(new byte[] { 1, 2, 3 }));
        }

        [Test]
        public async Task Handle_WhenApiThrowsException_ShouldReturnFailure()
        {
            // Arrange
            var workflowRunId = Guid.NewGuid();
            var exceptionMessage = "boom";

            _mockApiClient
                .Setup(x => x.Get<BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>>(
                    It.IsAny<GetRolloverCandidatesForExportApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            var query = new GetRolloverCandidatesForExportQuery
            {
                RolloverWorkflowRunId = workflowRunId
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));

            // Value should still be initialised
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.FileContent, Is.Empty);
            Assert.That(result.Value.FileName, Is.EqualTo(string.Empty));
            Assert.That(result.Value.ContentType, Is.EqualTo("text/csv"));
        }
    }
}
