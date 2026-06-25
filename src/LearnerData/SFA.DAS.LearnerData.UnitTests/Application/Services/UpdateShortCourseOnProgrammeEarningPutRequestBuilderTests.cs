using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class UpdateShortCourseOnProgrammeEarningPutRequestBuilderTests
{
    private UpdateShortCourseOnProgrammeEarningPutRequestBuilder _sut;

    [SetUp]
    public void SetUp() => _sut = new UpdateShortCourseOnProgrammeEarningPutRequestBuilder();

    [Test]
    public void Build_FromOnProgramme_Maps_Fields()
    {
        var onProgramme = new OnProgramme
        {
            WithdrawalDate = new DateTime(2025, 6, 1),
            CompletionDate = new DateTime(2025, 12, 1),
            Milestones = [Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete]
        };

        var result = _sut.Build(onProgramme);

        result.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
    }

    [Test]
    public void Build_FromShortCourseOnProgramme_Maps_Fields()
    {
        var learnerKey = Guid.NewGuid();
        const long ukprn = 12345678;
        const string learnerRef = "learner-ref";
        var onProgramme = new ShortCourseOnProgramme
        {
            WithdrawalDate = new DateTime(2025, 6, 1),
            CompletionDate = new DateTime(2025, 12, 1),
            Milestones = [Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete]
        };

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = learnerKey,
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = ukprn, StartDate = new DateTime(2025, 1, 1), LearnerRef = learnerRef }]
        };

        var result = _sut.Build(onProgramme, learningResponse, ukprn);

        result.WithdrawalDate.Should().Be(onProgramme.WithdrawalDate);
        result.CompletionDate.Should().Be(onProgramme.CompletionDate);
        result.Milestones.Should().BeEquivalentTo(onProgramme.Milestones);
    }

    [Test]
    public void Build_FromShortCourseOnProgramme_WithLearnerIdentity_Maps_Learner_Fields()
    {
        var learnerKey = Guid.NewGuid();
        const long ukprn = 12345678;
        const string learnerRef = "learner-ref-1";
        var onProgramme = new ShortCourseOnProgramme
        {
            WithdrawalDate = new DateTime(2025, 6, 1),
            CompletionDate = new DateTime(2025, 12, 1),
            Milestones = [Milestone.ThirtyPercentLearningComplete]
        };

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = learnerKey,
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = ukprn, StartDate = new DateTime(2025, 1, 1), LearnerRef = learnerRef }]
        };

        var result = _sut.Build(onProgramme, learningResponse, ukprn);

        result.LearnerKey.Should().Be(learnerKey);
        result.LearnerRef.Should().Be(learnerRef);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Adds_LearningComplete_When_CompletionDate_Set_And_Milestone_Absent(string overload)
    {
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = Guid.NewGuid(),
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = 12345678, StartDate = new DateTime(2025, 1, 1), LearnerRef = "learner-ref" }]
        };

        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = DateTime.UtcNow, Milestones = [Milestone.ThirtyPercentLearningComplete] })
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learningResponse, 12345678);

        result.Milestones.Should().Contain(Milestone.LearningComplete);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Does_Not_Duplicate_LearningComplete_When_Already_Present(string overload)
    {
        var milestones = new[] { Milestone.ThirtyPercentLearningComplete, Milestone.LearningComplete };
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = Guid.NewGuid(),
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = 12345678, StartDate = new DateTime(2025, 1, 1), LearnerRef = "learner-ref" }]
        };

        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = DateTime.UtcNow, Milestones = milestones.ToList() })
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = DateTime.UtcNow, Milestones = milestones }, learningResponse, 12345678);

        result.Milestones.Should().ContainSingle(m => m == Milestone.LearningComplete);
    }

    [TestCase(nameof(OnProgramme))]
    [TestCase(nameof(ShortCourseOnProgramme))]
    public void Build_Does_Not_Add_LearningComplete_When_CompletionDate_Not_Set(string overload)
    {
        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = Guid.NewGuid(),
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = 12345678, StartDate = new DateTime(2025, 1, 1), LearnerRef = "learner-ref" }]
        };

        var result = overload == nameof(OnProgramme)
            ? _sut.Build(new OnProgramme { CompletionDate = null, Milestones = [Milestone.ThirtyPercentLearningComplete] })
            : _sut.Build(new ShortCourseOnProgramme { CompletionDate = null, Milestones = [Milestone.ThirtyPercentLearningComplete] }, learningResponse, 12345678);

        result.Milestones.Should().NotContain(Milestone.LearningComplete);
    }

    [Test]
    public void Build_Throws_When_LearnerRef_Not_Found_For_Ukprn()
    {
        var onProgramme = new ShortCourseOnProgramme
        {
            CompletionDate = new DateTime(2025, 12, 1),
            Milestones = [Milestone.ThirtyPercentLearningComplete]
        };

        var learningResponse = new UpdateShortCourseLearningPutResponse
        {
            LearnerKey = Guid.NewGuid(),
            Episodes = [new LearningInnerShortCourseEpisode { Ukprn = 99999999, StartDate = new DateTime(2025, 1, 1), LearnerRef = "other-ref" }]
        };

        var act = () => _sut.Build(onProgramme, learningResponse, 12345678);

        act.Should().Throw<InvalidOperationException>();
    }
}
