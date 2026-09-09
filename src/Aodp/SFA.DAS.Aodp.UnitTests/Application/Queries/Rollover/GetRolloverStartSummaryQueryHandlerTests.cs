using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover
{
    [TestFixture]
    public class GetRolloverStartSummaryQueryHandlerTests
    {
        private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
        private GetRolloverStartSummaryQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _handler = new GetRolloverStartSummaryQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_WhenApiReturnsResult_ShouldReturnSuccessResponse()
        {
            var apiResponse = new GetRolloverStartSummaryQueryResponse
            {
                TotalCandidatesCount = 10
            };

            _mockApiClient
                .Setup(x => x.Get<GetRolloverStartSummaryQueryResponse>(
                    It.IsAny<GetRolloverStartSummaryApiRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetRolloverStartSummaryQuery(), CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse));
        }

        [Test]
        public async Task Handle_WhenApiReturnsNull_ShouldReturnSuccessWithNullValue()
        {
            _mockApiClient
                .Setup(x => x.Get<GetRolloverStartSummaryQueryResponse>(
                    It.IsAny<GetRolloverStartSummaryApiRequest>()))
                .ReturnsAsync((GetRolloverStartSummaryQueryResponse?)null);

            var result = await _handler.Handle(new GetRolloverStartSummaryQuery(), CancellationToken.None);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
        {
            _mockApiClient
                .Setup(x => x.Get<GetRolloverStartSummaryQueryResponse>(
                    It.IsAny<GetRolloverStartSummaryApiRequest>()))
                .ThrowsAsync(new Exception("boom"));

            var result = await _handler.Handle(new GetRolloverStartSummaryQuery(), CancellationToken.None);

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("boom"));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.InstanceOf<GetRolloverStartSummaryQueryResponse>());
        }
    }
}
