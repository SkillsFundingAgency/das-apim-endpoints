using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.CommitmentsInner;
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
    public void Build_Should_Identify_The_Learning_From_Our_Own_Response()
    {
        // Arrange
        var learning = new UpdateLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            ApprovalsApprenticeshipId = 65472475,
            LearningType = LearningType.FoundationApprenticeship
        };

        // Act
        var result = _sut.Build(learning, new ApprovalsResponse());

        // Assert
        result.LearningKey.Should().Be(learning.LearningKey);
        result.ApprenticeshipId.Should().Be(65472475);
        result.LearningType.Should().Be("FoundationApprenticeship");
    }

    [Test]
    public void Build_Should_Echo_Each_Change_Approvals_Responded_With()
    {
        // Arrange
        var approvals = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult
                {
                    ChangeType = "StartDate",
                    ApprovalStatus = "autoApproved",
                    Data = new ApprovalsChangeData { Old = "2026-09-01", New = "2026-09-15" }
                }
            ]
        };

        // Act
        var result = _sut.Build(new UpdateLearnerApiPutResponse(), approvals);

        // Assert
        result.Changes.Should().ContainSingle();
        result.Changes[0].ChangeType.Should().Be("StartDate");
        result.Changes[0].Data.Old.Should().Be("2026-09-01");
        result.Changes[0].Data.New.Should().Be("2026-09-15");
    }

    [Test]
    public void Build_Should_Echo_Every_Change_Approvals_Sent_Not_Only_Those_We_Asked_About()
    {
        // Arrange
        var approvals = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = "autoApproved", Data = new ApprovalsChangeData { Old = "a", New = "b" } },
                new ApprovalsChangeResult { ChangeType = "Firstname", ApprovalStatus = "autoApproved", Data = new ApprovalsChangeData { Old = "c", New = "d" } }
            ]
        };

        // Act
        var result = _sut.Build(new UpdateLearnerApiPutResponse(), approvals);

        // Assert
        result.Changes.Select(x => x.ChangeType).Should().Equal("StartDate", "Firstname");
    }

    [Test]
    public void Build_Should_Send_Empty_Strings_When_Old_New_Or_Data_Is_Missing()
    {
        // Arrange
        var approvals = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = "autoApproved", Data = new ApprovalsChangeData { Old = null, New = "2026-09-15" } },
                new ApprovalsChangeResult { ChangeType = "Firstname", ApprovalStatus = "autoApproved", Data = null }
            ]
        };

        // Act
        var result = _sut.Build(new UpdateLearnerApiPutResponse(), approvals);

        // Assert
        result.Changes[0].Data.Old.Should().BeEmpty();
        result.Changes[0].Data.New.Should().Be("2026-09-15");
        result.Changes[1].Data.Old.Should().BeEmpty();
        result.Changes[1].Data.New.Should().BeEmpty();
    }
}
