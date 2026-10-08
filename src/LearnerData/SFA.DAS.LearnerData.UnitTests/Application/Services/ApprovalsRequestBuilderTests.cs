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
        change.Data.Old.Should().BeNull();
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

        // Act
        var result = _sut.Build(10001234, 9999999999, response);

        // Assert
        result.Data.Changes.Should().BeEmpty();
        result.Data.NewPrices.Should().HaveCount(response.Prices.Count);
    }

    [Test]
    public void Build_Should_Not_Send_NewPrices_When_Neither_Prices_Nor_StartDate_Changed()
    {
        // Arrange
        var response = ResponseWith(BaseLearnerApiPutResponse.LearningUpdateChanges.ExpectedEndDate);

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
