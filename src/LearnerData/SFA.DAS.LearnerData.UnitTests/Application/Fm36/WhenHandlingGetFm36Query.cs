using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using static SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers.GetFm36QueryTestFixture;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36;

public class WhenHandlingGetFm36Query
{
#pragma warning disable CS8618 // initialised in setup
    private GetFm36QueryTestFixture _testFixture;
#pragma warning restore CS8618 // initialised in setup

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _testFixture = new GetFm36QueryTestFixture(TestScenario.AllData);

    }

    [Test]
    public async Task ThenCallsApprenticeshipsApiWithUnpagedQuery()
    {
        // Act
        await _testFixture.CallSubjectUnderTest();

        //Assert
        _testFixture.MockApprenticeshipsApiClient.Verify(x => x.Get<List<Learning>>(It.Is<GetLearningsRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
    }

    [Test]
    public async Task ThenCallsEarningsApiWithUnpagedQuery()
    {
        // Act
        await _testFixture.CallSubjectUnderTest();

        //Assert
        _testFixture.MockEarningsApiClient.Verify(x => x.PostWithResponseCode<GetFm36DataResponse>(It.Is<PostGetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn), It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public async Task ThenReturnsFM36LearnerIdentifiersWithUnpagedQuery()
    {
        // Act
        await _testFixture.CallSubjectUnderTest();

        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.UnpagedLearningsResponse)
        {
            _testFixture.Result.Items.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == "9999999999");
        }
    }

    [Test]
    public async Task ThenCallsApprenticeshipsApi()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        //Assert
        _testFixture.MockApprenticeshipsApiClient.Verify(x => x.Get<GetPagedLearnersFromLearningInner>(It.Is<GetLearningsRequest>(r => r.Ukprn == _testFixture.Ukprn)), Times.Once);
    }

    [Test]
    public async Task ThenCallsEarningsApi()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        //Assert
        _testFixture.MockEarningsApiClient.Verify(x => x.PostWithResponseCode<GetFm36DataResponse>(It.Is<PostGetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn), It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public async Task ThenReturnsFM36LearnerIdentifiers()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        // Assert
        _testFixture.Result.Should().NotBeNull();

        foreach (var apprenticeship in _testFixture.UnpagedLearningsResponse)
        {
            _testFixture.Result.Items.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == "9999999999");
        }
    }
}
