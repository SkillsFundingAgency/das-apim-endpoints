using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover
{
    [TestFixture]
    public class GetRolloverCandidatesQueryHandlerTests
    {
        private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
        private GetRolloverCandidatesQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _handler = new GetRolloverCandidatesQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_WhenApiReturnsResult_ShouldReturnSuccessResponse()
        {
            // Arrange
            var apiResponse = new GetRolloverCandidatesQueryResponse
            {
               RolloverCandidates = new List<RolloverCandidate>
               {
                   new RolloverCandidate
                   {
                       Id = Guid.NewGuid(),
                       QualificationNumber = "1234"
                   }
               }
            };

            _mockApiClient
                .Setup(x => x.Get<GetRolloverCandidatesQueryResponse>(
                    It.IsAny<GetRolloverCandidatesApiRequest>()))
                .ReturnsAsync(apiResponse);

            var handler = new GetRolloverCandidatesQueryHandler(_mockApiClient.Object);

            // Act
            var result = await handler.Handle(new GetRolloverCandidatesQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(apiResponse));
        }

        [Test]
        public async Task Handle_WhenApiReturnsNull_ShouldReturnFailureResponse()
        {
            // Arrange
            _mockApiClient
                .Setup(x => x.Get<GetRolloverCandidatesQueryResponse>(
                    It.IsAny<GetRolloverCandidatesApiRequest>()))
                .ReturnsAsync((GetRolloverCandidatesQueryResponse?)null);

            var handler = new GetRolloverCandidatesQueryHandler(_mockApiClient.Object);

            // Act
            var result = await handler.Handle(new GetRolloverCandidatesQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Failed to get rollover workflow candidates."));
        }

        [Test]
        public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
        {
            // Arrange
            _mockApiClient
                .Setup(x => x.Get<GetRolloverCandidatesQueryResponse>(
                    It.IsAny<GetRolloverCandidatesApiRequest>()))
                .ThrowsAsync(new Exception("boom"));

            var handler = new GetRolloverCandidatesQueryHandler(_mockApiClient.Object);

            // Act
            var result = await handler.Handle(new GetRolloverCandidatesQuery(), CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("boom")); // propagates ex.Message
        }
    }
}