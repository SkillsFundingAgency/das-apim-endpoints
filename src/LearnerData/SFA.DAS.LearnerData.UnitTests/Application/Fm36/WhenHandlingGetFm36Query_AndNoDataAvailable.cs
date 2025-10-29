using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

[TestFixture]
public class WhenHandlingGetFm36Query_AndNoDataAvailable
{
#pragma warning disable CS8618 // initialised in setup
    private GetFm36QueryTestFixture _testFixture;
#pragma warning restore CS8618 // initialised in setup

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _testFixture = new GetFm36QueryTestFixture(TestScenario.NoData);

        // Act
        await _testFixture.CallSubjectUnderTest();
    }

    [Test]
    public void EmptyArrayIsReturned()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();
        _testFixture.Result.Items.Should().BeEmpty();
    }
}
