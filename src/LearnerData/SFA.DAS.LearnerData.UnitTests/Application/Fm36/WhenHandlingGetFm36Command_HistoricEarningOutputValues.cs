using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

[TestFixture]
public class WhenHandlingGetFm36Command_HistoricEarningOutputValues
{
#pragma warning disable CS8618 // initialised in setup
    private GetFm36CommandTestFixture _testFixture;
#pragma warning restore CS8618 // initialised in setup

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _testFixture = new GetFm36CommandTestFixture(TestScenario.AllData);

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
