using FluentAssertions;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings
{
    [TestFixture]
    public class WhenHandlingGetAllEarningsQuery_HistoricEarningOutputValues
    {
        private GetAllEarningsQueryTestFixture _testFixture;

        [SetUp]
        public async Task SetUp()
        {
            // Arrange
            _testFixture = new GetAllEarningsQueryTestFixture(TestScenario.AllData);

            // Act
            await _testFixture.CallSubjectUnderTest();
        }

        [Test]
        public void EmptyArrayIsReturned()
        {
            // Assert
            _testFixture.Result.FM36Learners.Should().AllSatisfy(x => x.HistoricEarningOutputValues.Should().BeEmpty());
        }
    }
}
