using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
#pragma warning disable CS8618
    [TestFixture]
    public class WhenHandlingGetAllEarningsQuery_AndNoDataAvailable 
    { 
        private Fixture _fixture;
        private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
        private Mock<IEarningsApiClient<EarningsApiConfiguration>> _mockEarningsApiClient;
        private Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _mockCollectionCalendarApiClient;
        private Mock<ILogger<GetAllEarningsQueryHandler>> _mockLogger;
        private GetAllEarningsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _mockEarningsApiClient = new Mock<IEarningsApiClient<EarningsApiConfiguration>>();
            _mockCollectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();
            _mockLogger = new Mock<ILogger<GetAllEarningsQueryHandler>>();

            _handler = new GetAllEarningsQueryHandler(
                _mockApprenticeshipsApiClient.Object,
                _mockEarningsApiClient.Object,
                _mockCollectionCalendarApiClient.Object,
                _mockLogger.Object);
        }


        [Test]
        public async Task EmptyArrayIsReturned()
        {
            // Arrange / Act
            var result = await _handler.Handle(_fixture.Create<GetAllEarningsQuery>(), _fixture.Create<CancellationToken>());

            // Assert
            result.Should().NotBeNull();
            result.FM36Learners.Should().BeEmpty();
        }
    }
#pragma warning restore CS8618
}
