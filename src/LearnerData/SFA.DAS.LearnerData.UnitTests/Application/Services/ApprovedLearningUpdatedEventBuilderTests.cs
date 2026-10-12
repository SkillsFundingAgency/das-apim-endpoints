using System.Text.Json.Nodes;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ApprovedLearningUpdatedEventBuilderTests
{
    private ApprovedLearningUpdatedEventBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new ApprovedLearningUpdatedEventBuilder();
    }

    [Test]
    public void Build_Should_Map_StartDate_Change_With_Old_And_New()
    {
        // Arrange
        var response = new UpdateLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            ApprovalsApprenticeshipId = 65472475,
            LearningType = LearningType.FoundationApprenticeship,
            ChangeDetails =
            [
                new BaseLearnerApiPutResponse.ChangeDetail
                {
                    ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                    Old = JsonValue.Create("2026-09-01"),
                    New = JsonValue.Create("2026-09-15"),
                    RequiresApproval = true
                }
            ]
        };

        // Act
        var result = _sut.Build(response);

        // Assert
        result.LearningKey.Should().Be(response.LearningKey);
        result.ApprenticeshipId.Should().Be(65472475);
        result.LearningType.Should().Be("foundationapprenticeship");
        result.Changes.Should().ContainSingle();
        result.Changes[0].ChangeType.Should().Be("StartDate");
        result.Changes[0].Data.Old.Should().Be("2026-09-01");
        result.Changes[0].Data.New.Should().Be("2026-09-15");
    }

    [Test]
    public void Build_Should_Not_Include_Changes_That_Did_Not_Require_Approval()
    {
        // Arrange
        var response = new UpdateLearnerApiPutResponse
        {
            ChangeDetails =
            [
                new BaseLearnerApiPutResponse.ChangeDetail
                {
                    ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                    Old = JsonValue.Create("2026-09-01"),
                    New = JsonValue.Create("2026-09-15"),
                    RequiresApproval = false
                }
            ]
        };

        // Act
        var result = _sut.Build(response);

        // Assert
        result.Changes.Should().BeEmpty();
    }

    [Test]
    public void Build_Should_Send_Empty_Strings_When_Old_Or_New_Is_Missing()
    {
        // Arrange
        var response = new UpdateLearnerApiPutResponse
        {
            ChangeDetails =
            [
                new BaseLearnerApiPutResponse.ChangeDetail
                {
                    ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                    Old = null,
                    New = JsonValue.Create("2026-09-15"),
                    RequiresApproval = true
                }
            ]
        };

        // Act
        var result = _sut.Build(response);

        // Assert
        result.Changes.Should().ContainSingle();
        result.Changes[0].Data.Old.Should().BeEmpty();
        result.Changes[0].Data.New.Should().Be("2026-09-15");
    }
}
