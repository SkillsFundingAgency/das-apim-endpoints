using System.Text.Json.Nodes;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Requests.CommitmentsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class ApprovalsRequestBuilderTests
{
    private ApprovalsRequestBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new ApprovalsRequestBuilder();
    }

    [Test]
    public void Build_Should_Map_StartDate_Change_And_Full_Price_Structure()
    {
        // Arrange
        var response = new UpdateLearnerApiPutResponse
        {
            LearningKey = Guid.NewGuid(),
            ApprovalsApprenticeshipId = 65472475,
            IsApproved = true,
            LearningType = LearningType.FoundationApprenticeship,
            Changes =
            [
                BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                BaseLearnerApiPutResponse.LearningUpdateChanges.Prices
            ],
            ChangeDetails =
            [
                new BaseLearnerApiPutResponse.ChangeDetail
                {
                    ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
                    Old = JsonValue.Create("2026-09-01"),
                    New = JsonValue.Create("2026-09-15"),
                    RequiresApproval = true
                },
                new BaseLearnerApiPutResponse.ChangeDetail
                {
                    ChangeType = BaseLearnerApiPutResponse.LearningUpdateChanges.Prices
                }
            ],
            Prices =
            [
                new BaseLearnerApiPutResponse.EpisodePrice
                {
                    StartDate = new DateTime(2026, 9, 15),
                    EndDate = new DateTime(2026, 12, 31),
                    TrainingPrice = 15000m,
                    EndPointAssessmentPrice = 2000m,
                    TotalPrice = 17000m
                },
                new BaseLearnerApiPutResponse.EpisodePrice
                {
                    StartDate = new DateTime(2027, 1, 1),
                    EndDate = new DateTime(2027, 8, 31),
                    TrainingPrice = 16000.5m,
                    EndPointAssessmentPrice = 2000m,
                    TotalPrice = 18000.5m
                }
            ]
        };

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        result.PutUrl.Should().Be($"approvals/{response.LearningKey}");

        var body = result.Data;
        body.LearningKey.Should().Be(response.LearningKey);
        body.ApprenticeshipId.Should().Be(65472475);
        body.LearningType.Should().Be("FoundationApprenticeship");
        body.Ukprn.Should().Be("10001234");
        body.Uln.Should().Be("9999999999");

        body.Changes.Should().ContainSingle();
        var change = body.Changes.Single();
        change.ChangeType.Should().Be("StartDate");
        change.Data.Old.Should().Be("2026-09-01");
        change.Data.New.Should().Be("2026-09-15");

        body.NewPrices.Should().BeEquivalentTo(
        [
            new ApprovalsPrice { TrainingPrice = "15000.00", AssessmentPrice = "2000.00", EffectiveFrom = new DateTime(2026, 9, 15) },
            new ApprovalsPrice { TrainingPrice = "16000.50", AssessmentPrice = "2000.00", EffectiveFrom = new DateTime(2027, 1, 1) }
        ], options => options.WithStrictOrdering());
    }

    [Test]
    public void Build_Should_Send_NewPrices_But_No_Field_Change_When_Only_Prices_Changed()
    {
        // Arrange
        var response = ResponseWith(BaseLearnerApiPutResponse.LearningUpdateChanges.Prices);
        response.Prices =
        [
            new BaseLearnerApiPutResponse.EpisodePrice
            {
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2027, 1, 31),
                TrainingPrice = 15000m,
                EndPointAssessmentPrice = 2000m,
                TotalPrice = 17000m
            },
            new BaseLearnerApiPutResponse.EpisodePrice
            {
                StartDate = new DateTime(2027, 2, 1),
                EndDate = new DateTime(2027, 8, 31),
                TrainingPrice = 18000m,
                EndPointAssessmentPrice = null,
                TotalPrice = 18000m
            }
        ];

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        result.Data.Changes.Should().BeEmpty();
        result.Data.NewPrices.Should().BeEquivalentTo(
        [
            new ApprovalsPrice { TrainingPrice = "15000.00", AssessmentPrice = "2000.00", EffectiveFrom = new DateTime(2026, 9, 1) },
            new ApprovalsPrice { TrainingPrice = "18000.00", AssessmentPrice = "0.00", EffectiveFrom = new DateTime(2027, 2, 1) }
        ], options => options.WithStrictOrdering());
    }

    [Test]
    public void Build_Should_Map_PlannedEndDate_Change_Without_Prices_When_Only_The_Planned_End_Date_Changed()
    {
        // Arrange
        var response = ResponseWith(BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);
        response.ChangeDetails.Single().Old = JsonValue.Create("2027-07-31");
        response.ChangeDetails.Single().New = JsonValue.Create("2027-09-30");

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        var change = result.Data.Changes.Should().ContainSingle().Subject;
        change.ChangeType.Should().Be("PlannedEndDate");
        change.Data.Old.Should().Be("2027-07-31");
        change.Data.New.Should().Be("2027-09-30");
        result.Data.NewPrices.Should().BeEmpty();
    }

    [Test]
    public void Build_Should_Map_StartDate_And_PlannedEndDate_Changes_And_Prices_When_All_Changed_Together()
    {
        // Arrange
        var response = ResponseWith(
            BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate,
            BaseLearnerApiPutResponse.LearningUpdateChanges.Prices,
            BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);

        var startDate = response.ChangeDetails.Single(x => x.ChangeType == BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate);
        startDate.Old = JsonValue.Create("2026-08-15");
        startDate.New = JsonValue.Create("2026-09-01");

        var plannedEndDate = response.ChangeDetails.Single(x => x.ChangeType == BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);
        plannedEndDate.Old = JsonValue.Create("2027-07-31");
        plannedEndDate.New = JsonValue.Create("2027-08-31");

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        result.Data.Changes.Select(x => (x.ChangeType, x.Data.Old, x.Data.New)).Should().BeEquivalentTo(
        [
            ("StartDate", "2026-08-15", "2026-09-01"),
            ("PlannedEndDate", "2027-07-31", "2027-08-31")
        ], options => options.WithStrictOrdering());

        result.Data.NewPrices.Should().BeEquivalentTo(
        [
            new ApprovalsPrice { TrainingPrice = "15000.00", AssessmentPrice = "2000.00", EffectiveFrom = new DateTime(2026, 9, 1) }
        ]);
    }

    [TestCase(BaseLearnerApiPutResponse.LearningUpdateChanges.StartDate)]
    [TestCase(BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate)]
    public void Build_Should_Not_Send_A_Field_Change_For_A_Change_That_Does_Not_Need_Approval(BaseLearnerApiPutResponse.LearningUpdateChanges changeType)
    {
        // Arrange
        var response = ResponseWith(changeType);
        response.ChangeDetails.ForEach(x => x.RequiresApproval = false);

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        result.Data.Changes.Should().BeEmpty();
        result.Data.NewPrices.Should().BeEmpty();
    }

    private static UpdateLearnerApiPutResponse ResponseWith(params BaseLearnerApiPutResponse.LearningUpdateChanges[] changes) => new()
    {
        LearningKey = Guid.NewGuid(),
        ApprovalsApprenticeshipId = 65472475,
        IsApproved = true,
        LearningType = LearningType.Apprenticeship,
        Changes = [.. changes],
        ChangeDetails = changes
            .Select(x => new BaseLearnerApiPutResponse.ChangeDetail { ChangeType = x, RequiresApproval = true })
            .ToList(),
        Prices =
        [
            new BaseLearnerApiPutResponse.EpisodePrice
            {
                StartDate = new DateTime(2026, 9, 1),
                EndDate = new DateTime(2027, 8, 31),
                TrainingPrice = 15000m,
                EndPointAssessmentPrice = 2000m,
                TotalPrice = 17000m
            }
        ]
    };
}
