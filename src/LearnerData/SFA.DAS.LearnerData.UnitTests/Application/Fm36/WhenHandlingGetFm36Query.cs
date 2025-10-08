using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

public class WhenHandlingGetFm36Query
{
#pragma warning disable CS8618 // initialised in setup
    private GetFm36QueryTestFixture _testFixture;
#pragma warning restore CS8618 // initialised in setup

    [SetUp]
    public async Task SetUp()
    {
        // Arrange
        _testFixture = new GetFm36QueryTestFixture(TestScenario.AllData);

        // Act
        await _testFixture.CallSubjectUnderTest();
    }

    [Test]
    public void ThenCallsApprenticeshipsApi()
    {
        //Assert
        _testFixture.MockApprenticeshipsApiClient.Verify(x => x.Get<GetLearningsResponse>(It.Is<GetLearningsRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
    }

    [Test]
    public void ThenCallsEarningsApi()
    {
        //Assert
        _testFixture.MockEarningsApiClient.Verify(x => x.Get<GetFm36DataResponse>(It.Is<GetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
    }

    [Test]
    public void ThenReturnsFM36LearnerIdentifiers()
    {
        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.LearningsResponse.Learnings)
        {
            _testFixture.Result.FM36Learners.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == "9999999999");
        }
    }
}
