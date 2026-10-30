using AutoFixture;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class CreateUnapprovedApprenticeshipLearningRequestBuilderTests
{
    private readonly Fixture _fixture = new();
    private Mock<ICourseService> _courseService = null!;
    private CreateUnapprovedApprenticeshipLearningRequestBuilder _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _courseService = new Mock<ICourseService>();
        _sut = new CreateUnapprovedApprenticeshipLearningRequestBuilder(_courseService.Object);

        _courseService
            .Setup(x => x.GetStandardDetailsById(It.IsAny<string>()))
            .ReturnsAsync(new StandardDetailResponse
            {
                ApprenticeshipFunding =
                [
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = new DateTime(2020, 1, 1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = 15000
                    }
                ]
            });
    }

    [Test]
    public async Task Build_Should_Create_Post_Request_For_Earnings_Inner()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = _fixture.Build<CreateDraftLearnerApiPutResponse>()
            .With(x => x.Changes, new List<BaseLearnerApiPutResponse.LearningUpdateChanges>())
            .Create();

        var request = _fixture.Create<CreateLearnerRequest>();
        var firstOnProgramme = request.Delivery.OnProgramme.First();
        request.Delivery.OnProgramme =
        [
            firstOnProgramme,
            _fixture.Build<CreateLearnerRequest.OnProgrammeDetails>()
                .With(x => x.StandardCode, firstOnProgramme.StandardCode)
                .With(x => x.AgreementId, firstOnProgramme.AgreementId)
                .Create()
        ];

        var requestBody = _fixture.Create<UpdateLearningRequestBody>();

        // Act
        var result = await _sut.Build(ukprn, request, learningResponse, requestBody);

        // Assert
        result.PostUrl.Should().Be("learning");

        var payload = result.Data as SFA.DAS.LearnerData.Requests.EarningsInner.CreateUnapprovedApprenticeshipLearningRequest;
        payload.Should().NotBeNull();

        payload!.LearningKey.Should().Be(learningResponse.LearningKey);
        payload.EpisodeKey.Should().Be(learningResponse.LearningEpisodeKey);
        payload.OnProgramme.Ukprn.Should().Be(ukprn);
        payload.OnProgramme.TrainingCode.Should().Be(firstOnProgramme.StandardCode.ToString());
        payload.OnProgramme.EmployerType.Should().Be(SFA.DAS.LearnerData.Requests.EarningsInner.ApprenticeshipFundingType.Levy);
        payload.OnProgramme.FundingBandMaximum.Should().Be(15000);
        payload.Learner.Uln.Should().Be(requestBody.Learner.Uln.ToString());
        payload.Prices.Should().HaveCount(learningResponse.Prices.Count);
        payload.IsNewApprenticeshipLearner.Should().BeFalse();
    }

    [Test]
    public async Task Build_Should_Set_IsNewApprenticeshipLearner_When_Learning_Response_Contains_NewApprenticeshipLearner_Change()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = _fixture.Build<CreateDraftLearnerApiPutResponse>()
            .With(x => x.Changes, new List<BaseLearnerApiPutResponse.LearningUpdateChanges> { BaseLearnerApiPutResponse.LearningUpdateChanges.NewApprenticeshipLearner })
            .Create();

        var request = _fixture.Create<CreateLearnerRequest>();
        var requestBody = _fixture.Create<UpdateLearningRequestBody>();

        // Act
        var result = await _sut.Build(ukprn, request, learningResponse, requestBody);

        // Assert
        var payload = (SFA.DAS.LearnerData.Requests.EarningsInner.CreateUnapprovedApprenticeshipLearningRequest)result.Data;
        payload.IsNewApprenticeshipLearner.Should().BeTrue();
    }

    [Test]
    public async Task Build_Should_Map_OnProgramme_Periods_From_Same_Agreement_And_Standard()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = _fixture.Create<CreateDraftLearnerApiPutResponse>();
        var baseStartDate = new DateTime(2025, 8, 1);

        var match = _fixture.Build<CreateLearnerRequest.OnProgrammeDetails>()
            .With(x => x.StandardCode, 123)
            .With(x => x.AgreementId, "A")
            .With(x => x.StartDate, baseStartDate)
            .With(x => x.CompletionDate, (DateTime?)null)
            .With(x => x.PauseDate, (DateTime?)null)
            .With(x => x.WithdrawalDate, (DateTime?)null)
            .Create();

        var match2 = _fixture.Build<CreateLearnerRequest.OnProgrammeDetails>()
            .With(x => x.StandardCode, 123)
            .With(x => x.AgreementId, "A")
            .With(x => x.StartDate, baseStartDate.AddMonths(6))
            .With(x => x.PauseDate, baseStartDate.AddMonths(10))
            .Create();

        var nonMatch = _fixture.Build<CreateLearnerRequest.OnProgrammeDetails>()
            .With(x => x.StandardCode, 456)
            .With(x => x.AgreementId, "B")
            .With(x => x.StartDate, baseStartDate.AddYears(2))
            .Create();

        var request = _fixture.Build<CreateLearnerRequest>()
            .With(x => x.Delivery, new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = [match, match2, nonMatch],
                EnglishAndMaths = _fixture.CreateMany<MathsAndEnglish>().ToList()
            })
            .Create();

        var requestBody = _fixture.Create<UpdateLearningRequestBody>();

        // Act
        var result = await _sut.Build(ukprn, request, learningResponse, requestBody);

        // Assert
        var payload = (SFA.DAS.LearnerData.Requests.EarningsInner.CreateUnapprovedApprenticeshipLearningRequest)result.Data;
        payload.PeriodsInLearning.Should().HaveCount(2);
        payload.PeriodsInLearning.Select(x => x.StartDate).Should().Contain(match.StartDate);
        payload.PeriodsInLearning.Select(x => x.StartDate).Should().Contain(match2.StartDate);
    }

    [Test]
    public async Task Build_Should_Map_EnglishAndMaths_Periods_From_Original_Items_When_There_Is_A_Break_In_Learning()
    {
        // Arrange
        var ukprn = _fixture.Create<long>();
        var learningResponse = _fixture.Create<CreateDraftLearnerApiPutResponse>();

        var firstStart = new DateTime(2025, 9, 1);
        var firstPause = new DateTime(2025, 12, 15);
        var firstEnd = new DateTime(2026, 3, 31);
        var secondStart = new DateTime(2026, 2, 1);
        var secondEnd = new DateTime(2026, 6, 30);

        var firstItem = _fixture.Build<MathsAndEnglish>()
            .With(x => x.LearnAimRef, "AIM1")
            .With(x => x.StartDate, firstStart)
            .With(x => x.EndDate, firstEnd)
            .With(x => x.PauseDate, firstPause)
            .With(x => x.CompletionDate, (DateTime?)null)
            .With(x => x.WithdrawalDate, (DateTime?)null)
            .Create();

        var secondItem = _fixture.Build<MathsAndEnglish>()
            .With(x => x.LearnAimRef, "AIM1")
            .With(x => x.StartDate, secondStart)
            .With(x => x.EndDate, secondEnd)
            .With(x => x.PauseDate, (DateTime?)null)
            .With(x => x.CompletionDate, (DateTime?)null)
            .With(x => x.WithdrawalDate, (DateTime?)null)
            .Create();

        var request = _fixture.Build<CreateLearnerRequest>()
            .With(x => x.Delivery, new CreateLearnerRequest.DeliveryDetails
            {
                OnProgramme = _fixture.CreateMany<CreateLearnerRequest.OnProgrammeDetails>(1).ToList(),
                EnglishAndMaths = [secondItem, firstItem]
            })
            .Create();

        var requestBody = _fixture.Create<UpdateLearningRequestBody>();
        requestBody.EnglishAndMathsCourses =
        [
            new MathsAndEnglishDetails
            {
                LearnAimRef = "AIM1",
                StartDate = firstStart,
                PlannedEndDate = secondEnd,
                LearningSupport = [],
                BreaksInLearning =
                [
                    new BreakInLearning
                    {
                        StartDate = firstPause.AddDays(1),
                        EndDate = secondStart.AddDays(-1),
                        PriorPeriodExpectedEndDate = firstEnd
                    }
                ]
            }
        ];

        // Act
        var result = await _sut.Build(ukprn, request, learningResponse, requestBody);

        // Assert
        var payload = (SFA.DAS.LearnerData.Requests.EarningsInner.CreateUnapprovedApprenticeshipLearningRequest)result.Data;
        var periods = payload.EnglishAndMaths.Single().PeriodsInLearning;

        periods.Should().HaveCount(2);
        periods[0].StartDate.Should().Be(firstStart);
        periods[0].EndDate.Should().Be(firstPause);
        periods[0].OriginalExpectedEndDate.Should().Be(firstEnd);
        periods[1].StartDate.Should().Be(secondStart);
        periods[1].EndDate.Should().BeNull();
        periods[1].OriginalExpectedEndDate.Should().Be(secondEnd);
    }
}
