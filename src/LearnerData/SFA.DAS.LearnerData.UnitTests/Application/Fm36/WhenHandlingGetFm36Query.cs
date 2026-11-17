using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.TestHelpers;
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
    public async Task ThenCallsLearningApiWithTheKeysReturnedByEarningsWithUnpagedQuery()
    {
        // Act
        await _testFixture.CallSubjectUnderTest();

        //Assert
        var expectedKeys = _testFixture.EarningsResponse.Apprenticeships.Select(x => x.Key).ToList();
        _testFixture.MockApprenticeshipsApiClient.Verify(x => x.PostWithResponseCode<List<Learning>>(
            It.Is<PostGetLearningsRequest>(r => r.Ukprn == _testFixture.Ukprn && ((List<Guid>)r.Data).SequenceEqual(expectedKeys)), It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public async Task ThenCallsEarningsApiWithUnpagedQuery()
    {
        // Act
        await _testFixture.CallSubjectUnderTest();

        //Assert
        _testFixture.MockEarningsApiClient.Verify(x => x.PostWithResponseCode<GetFm36DataResponse>(
            It.Is<PostGetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn && r.Page == null && r.PageSize == null && r.PostUrl.Contains("?") == false), It.IsAny<bool>()), Times.Once);
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
            _testFixture.Result.Items.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == DefaultLearner.LearnerRef);
        }
    }

    [Test]
    public async Task ThenCallsEarningsApiWithPagingParameters()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        //Assert
        _testFixture.MockEarningsApiClient.Verify(x => x.PostWithResponseCode<GetFm36DataResponse>(
            It.Is<PostGetFm36DataRequest>(r => r.Ukprn == _testFixture.Ukprn && r.Page == 1 && r.PageSize == 1 && r.PostUrl.EndsWith("?page=1&pageSize=1")), It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public async Task ThenCallsLearningApiWithTheKeysReturnedByEarnings()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        //Assert
        _testFixture.MockApprenticeshipsApiClient.Verify(x => x.PostWithResponseCode<List<Learning>>(It.Is<PostGetLearningsRequest>(r => r.Ukprn == _testFixture.Ukprn), It.IsAny<bool>()), Times.Once);
    }

    [Test]
    public async Task ThenCallsEarningsBeforeLearning()
    {
        // Arrange
        var callOrder = new List<string>();

        _testFixture.MockEarningsApiClient
            .Setup(x => x.PostWithResponseCode<GetFm36DataResponse>(It.IsAny<PostGetFm36DataRequest>(), It.IsAny<bool>()))
            .Callback(() => callOrder.Add("earnings"))
            .ReturnsAsync(new ApiResponse<GetFm36DataResponse>(_testFixture.EarningsResponse, System.Net.HttpStatusCode.OK, string.Empty));

        _testFixture.MockApprenticeshipsApiClient
            .Setup(x => x.PostWithResponseCode<List<Learning>>(It.IsAny<PostGetLearningsRequest>(), It.IsAny<bool>()))
            .Callback(() => callOrder.Add("learning"))
            .ReturnsAsync(new ApiResponse<List<Learning>>(_testFixture.UnpagedLearningsResponse, System.Net.HttpStatusCode.OK, string.Empty));

        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        // Assert
        callOrder.Should().Equal("earnings", "learning");
    }

    [Test]
    public async Task ThenThrowsWhenLearningIsMissingForAnEarningsKey()
    {
        // Arrange
        _testFixture.MockApprenticeshipsApiClient
            .Setup(x => x.PostWithResponseCode<List<Learning>>(It.IsAny<PostGetLearningsRequest>(), It.IsAny<bool>()))
            .ReturnsAsync(new ApiResponse<List<Learning>>(null!, System.Net.HttpStatusCode.NotFound, string.Empty));

        // Act
        var act = () => _testFixture.CallSubjectUnderTest(QueryType.Paged);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Learning data missing for earnings key*");
    }

    [Test]
    public async Task ThenReturnsTotalItemsFromEarnings()
    {
        // Act
        await _testFixture.CallSubjectUnderTest(QueryType.Paged);

        // Assert
        _testFixture.Result.TotalItems.Should().Be(_testFixture.EarningsResponse.TotalItems);
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
            _testFixture.Result.Items.Should().Contain(learner => learner.ULN == long.Parse(apprenticeship.Uln) && learner.LearnRefNumber == DefaultLearner.LearnerRef);
        }
    }
}
