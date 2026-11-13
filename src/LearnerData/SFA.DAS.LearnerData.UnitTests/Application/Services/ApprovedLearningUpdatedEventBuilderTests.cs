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

    [TestCase("autoApproved")]
    [TestCase("AutoApproved")]
    public void Build_Should_Echo_Our_Prices_And_No_Prices_Change_Item_When_The_Prices_Are_Auto_Approved(string status)
    {
        // Arrange
        var learning = new UpdateLearnerApiPutResponse
        {
            Prices =
            [
                new UpdateLearnerApiPutResponse.EpisodePrice { StartDate = new DateTime(2026, 9, 1), TrainingPrice = 12000, EndPointAssessmentPrice = 3000 },
                new UpdateLearnerApiPutResponse.EpisodePrice { StartDate = new DateTime(2027, 2, 1), TrainingPrice = 14000, EndPointAssessmentPrice = null }
            ]
        };
        var approvals = new ApprovalsResponse
        {
            Changes = [new ApprovalsChangeResult { ChangeType = "Prices", ApprovalStatus = status, Data = null }]
        };

        // Act
        var result = _sut.Build(learning, approvals);

        // Assert
        result.Changes.Should().BeEmpty();
        result.Prices.Should().HaveCount(2);
        result.Prices[0].EffectiveFrom.Should().Be(new DateTime(2026, 9, 1));
        result.Prices[0].TrainingPrice.Should().Be(12000);
        result.Prices[0].AssessmentPrice.Should().Be(3000);
        result.Prices[1].EffectiveFrom.Should().Be(new DateTime(2027, 2, 1));
        result.Prices[1].TrainingPrice.Should().Be(14000);
        result.Prices[1].AssessmentPrice.Should().Be(0);
    }

    [TestCase("pending")]
    [TestCase("employerApprovalRequested")]
    [TestCase("autoRejected")]
    public void Build_Should_Not_Echo_Prices_That_Were_Not_Auto_Approved(string status)
    {
        // Arrange
        var learning = new UpdateLearnerApiPutResponse
        {
            Prices = [new UpdateLearnerApiPutResponse.EpisodePrice { StartDate = new DateTime(2026, 9, 1), TrainingPrice = 12000, EndPointAssessmentPrice = 3000 }]
        };
        var approvals = new ApprovalsResponse
        {
            Changes = [new ApprovalsChangeResult { ChangeType = "Prices", ApprovalStatus = status, Data = null }]
        };

        // Act
        var result = _sut.Build(learning, approvals);

        // Assert
        result.Prices.Should().BeEmpty();
        result.Changes.Should().BeEmpty();
    }

    [TestCase("pending")]
    [TestCase("employerApprovalRequested")]
    [TestCase("autoRejected")]
    public void Build_Should_Echo_Only_The_Auto_Approved_Changes(string otherStatus)
    {
        // Arrange
        var learning = new UpdateLearnerApiPutResponse
        {
            Prices = [new UpdateLearnerApiPutResponse.EpisodePrice { StartDate = new DateTime(2026, 9, 15), TrainingPrice = 12000, EndPointAssessmentPrice = 3000 }]
        };
        var approvals = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = otherStatus, Data = new ApprovalsChangeData { Old = "2026-09-01", New = "2026-09-15" } },
                new ApprovalsChangeResult { ChangeType = "Firstname", ApprovalStatus = "autoApproved", Data = new ApprovalsChangeData { Old = "c", New = "d" } },
                new ApprovalsChangeResult { ChangeType = "Prices", ApprovalStatus = otherStatus, Data = null }
            ]
        };

        // Act
        var result = _sut.Build(learning, approvals);

        // Assert
        result.Changes.Select(x => x.ChangeType).Should().Equal("Firstname");
        result.Prices.Should().BeEmpty();
    }

    [Test]
    public void Build_Should_Echo_An_Auto_Approved_Start_Date_Without_The_Prices_That_Are_Pending()
    {
        // Arrange
        var learning = new UpdateLearnerApiPutResponse
        {
            Prices = [new UpdateLearnerApiPutResponse.EpisodePrice { StartDate = new DateTime(2026, 9, 15), TrainingPrice = 12000, EndPointAssessmentPrice = 3000 }]
        };
        var approvals = new ApprovalsResponse
        {
            Changes =
            [
                new ApprovalsChangeResult { ChangeType = "StartDate", ApprovalStatus = "autoApproved", Data = new ApprovalsChangeData { Old = "2026-09-01", New = "2026-09-15" } },
                new ApprovalsChangeResult { ChangeType = "Prices", ApprovalStatus = "employerApprovalRequested", Data = null }
            ]
        };

        // Act
        var result = _sut.Build(learning, approvals);

        // Assert
        result.Changes.Select(x => x.ChangeType).Should().Equal("StartDate");
        result.Prices.Should().BeEmpty();
    }
}
