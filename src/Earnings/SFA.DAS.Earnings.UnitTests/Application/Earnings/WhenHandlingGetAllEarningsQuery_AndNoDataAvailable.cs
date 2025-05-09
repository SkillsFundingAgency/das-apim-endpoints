using FluentAssertions;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{

    [TestFixture]
    public class WhenHandlingGetAllEarningsQuery_AndNoDataAvailable 
    {
        private GetAllEarningsQueryTestFixture _testFixture;

        [SetUp]
        public async Task SetUp()
        {
            // Arrange
            _testFixture = new GetAllEarningsQueryTestFixture(TestScenario.NoData);

            // Act
            await _testFixture.CallSubjectUnderTest();
        }

        [Test]
        public void EmptyArrayIsReturned()
        {
            // Assert
            _testFixture.Result.Should().NotBeNull();
            _testFixture.Result.FM36Learners.Should().BeEmpty();
        }
    }
}
