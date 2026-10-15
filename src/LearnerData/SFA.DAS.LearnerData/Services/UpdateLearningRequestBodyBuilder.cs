using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateLearningRequestBodyBuilder
{
    Task<UpdateLearningRequestBody> Build(long ukprn, UpdateLearnerRequest updateLearnerRequest);
    Task<UpdateLearningRequestBody> Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear, LearningType learningType);
}

public class UpdateLearningRequestBodyBuilder(
    ILearningSupportService learningSupportService,
    IBreaksInLearningService breaksInLearningService,
    ICostsService costsService,
    IEnglishAndMathsValueService englishAndMathsValueService)
    : IUpdateLearningRequestBodyBuilder
{
    public Task<UpdateLearningRequestBody> Build(long ukprn, UpdateLearnerRequest updateLearnerRequest)
    {
        return BuildInternal(
            ukprn,
            updateLearnerRequest.Learner,
            updateLearnerRequest.Delivery.EnglishAndMaths,
            updateLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(),
            updateLearnerRequest.EnglishAndMathsLearningSupport(),
            academicYear: 0);
    }

    public Task<UpdateLearningRequestBody> Build(long ukprn, CreateLearnerRequest createLearnerRequest, int academicYear, LearningType learningType)
    {
        return BuildInternal(
            ukprn,
            createLearnerRequest.Learner,
            createLearnerRequest.Delivery.EnglishAndMaths?.Cast<MathsAndEnglish>().ToList(),
            createLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(),
            createLearnerRequest.EnglishAndMathsLearningSupport(),
            academicYear,
            learningType);
    }

    private async Task<UpdateLearningRequestBody> BuildInternal(long ukprn,
        LearnerRequestDetails learner,
        List<MathsAndEnglish> englishAndMaths,
        List<OnProgrammeRequestDetails> onProgramme,
        List<KeyValuePair<string, List<LearningSupport>>> englishAndMathsLearningSupport,
        int academicYear,
        LearningType? learningType = null)
    {
        var (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes) = SelectEpisode(onProgramme);

        var costs = costsService.GetCosts(allMatchingOnProgrammes);
        var onProgrammeDetails = BuildOnProgrammeDetails(firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes, costs);
        var englishAndMathsCourses = await BuildEnglishAndMathsDetails(englishAndMaths ?? new List<MathsAndEnglish>());

        //Determine the effective end date of the latest OnProgramme
        var onProgrammeEndDate = new[]
        {
            latestOnProgramme.ExpectedEndDate,
            latestOnProgramme.CompletionDate ?? DateTime.MaxValue,
            latestOnProgramme.WithdrawalDate ?? DateTime.MaxValue,
            latestOnProgramme.PauseDate ?? DateTime.MaxValue
        }.Min();

        var (onProgrammeLearningSupport, englishAndMathsLearningSupportByCourse) = learningSupportService.GetLearningSupport(
            allMatchingOnProgrammes,
            onProgrammeEndDate,
            onProgrammeDetails.BreaksInLearning,
            englishAndMathsCourses,
            englishAndMathsLearningSupport);

        onProgrammeDetails.LearningSupport = onProgrammeLearningSupport ?? new List<LearningSupport>();
        onProgrammeDetails.WithdrawalDate = latestOnProgramme.WithdrawalDate;
        onProgrammeDetails.TrainingCode = firstOnProgramme.StandardCode.ToString();

        foreach (var course in englishAndMathsCourses)
        {
            course.LearningSupport = (englishAndMathsLearningSupportByCourse ?? new())
                .GetValueOrDefault(course.LearnAimRef, new List<LearningSupport>());
        }

        return new UpdateLearningRequestBody
        {
            AcademicYear = academicYear,
            Delivery = new Delivery
            {
                LearningType = learningType
            },
            Learner = new LearningUpdateDetails
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                EmailAddress = learner.Email,
                CompletionDate = latestOnProgramme.CompletionDate,
                DateOfBirth = learner.Dob,
                Uln = learner.Uln,
                LearnerRef = learner.LearnerRef,
                Care = new CareDetails
                {
                    HasEHCP = learner.HasEhcp,
                    IsCareLeaver = latestOnProgramme.Care.Careleaver,
                    CareLeaverEmployerConsentGiven = latestOnProgramme.Care.EmployerConsent
                }
            },
            OnProgramme = onProgrammeDetails,
            EnglishAndMathsCourses = englishAndMathsCourses
        };
    }

    private static (OnProgrammeRequestDetails FirstOnProgramme,
        OnProgrammeRequestDetails LatestOnProgramme,
        List<OnProgrammeRequestDetails> MatchingOnProgrammes) SelectEpisode(List<OnProgrammeRequestDetails> onProgramme)
    {
        var orderedOnProgrammes = onProgramme
            .OrderBy(x => x.StartDate)
            .ToList();

        var firstOnProgramme = orderedOnProgrammes.First();

        var allMatchingOnProgrammes = orderedOnProgrammes
            .Where(x => x.StandardCode == firstOnProgramme.StandardCode &&
                        x.AgreementId == firstOnProgramme.AgreementId)
            .ToList();

        var latestOnProgramme = allMatchingOnProgrammes.Last();

        return (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes);
    }

    private OnProgrammeDetails BuildOnProgrammeDetails(
        OnProgrammeRequestDetails firstOnProgramme, 
        OnProgrammeRequestDetails latestOnProgramme,
        List<OnProgrammeRequestDetails> allMatchingOnProgrammes,
        List<CostDetails> costs)
    {
        var breaksInLearning = breaksInLearningService.CalculateOnProgrammeBreaksInLearning(allMatchingOnProgrammes);

        return new OnProgrammeDetails
        {
            AchievementDate = latestOnProgramme.AchievementDate,
            ExpectedEndDate = latestOnProgramme.ExpectedEndDate,
            Costs = costs.GetCostsOrDefault(firstOnProgramme.StartDate),
            PauseDate = latestOnProgramme.PauseDate,
            BreaksInLearning = breaksInLearning
        };
    }

    private async Task<List<MathsAndEnglishDetails>> BuildEnglishAndMathsDetails(List<MathsAndEnglish> mathsAndEnglish)
    {
        var groupedCourses = mathsAndEnglish.GroupBy(c => c.LearnAimRef);
        var englishAndMathsCourses = new List<MathsAndEnglishDetails>();

        foreach (var g in groupedCourses)
        {
            if(g.Count() == 1)
            {
                var course = g.First();
                englishAndMathsCourses.Add(new MathsAndEnglishDetails
                {
                    Amount = await englishAndMathsValueService.GetValue(course.LearnAimRef, course.StartDate),
                    CompletionDate = course.CompletionDate,
                    LearnAimRef = course.LearnAimRef,
                    Course = course.Course,
                    PlannedEndDate = course.EndDate,
                    CombinedFundingAdjustmentPercentage = ResolveCombinedFundingAdjustmentPercentage(course.PriorLearningAdjustment,course.OtherFundingAdjustment),
                    StartDate = course.StartDate,
                    WithdrawalDate = course.WithdrawalDate,
                    PauseDate = course.PauseDate,
                    BreaksInLearning = new List<BreakInLearning>()
                });
            }
            else
            {
                var breaksInLearning = breaksInLearningService.CalculateEnglishAndMathsBreaksInLearning(g.ToList());
                var orderedCourses = g.OrderBy(c => c.StartDate).ToList();
                var firstCourse = orderedCourses.First();
                var latestCourse = orderedCourses.Last();

                englishAndMathsCourses.Add(new MathsAndEnglishDetails
                {
                    Amount = await englishAndMathsValueService.GetValue(firstCourse.LearnAimRef, firstCourse.StartDate),
                    CompletionDate = latestCourse.CompletionDate,
                    LearnAimRef = latestCourse.LearnAimRef,
                    Course = latestCourse.Course,
                    PlannedEndDate = latestCourse.EndDate,
                    CombinedFundingAdjustmentPercentage = ResolveCombinedFundingAdjustmentPercentage(latestCourse.PriorLearningAdjustment,latestCourse.OtherFundingAdjustment),
                    StartDate = firstCourse.StartDate,
                    WithdrawalDate = latestCourse.WithdrawalDate,
                    PauseDate = latestCourse.PauseDate,
                    BreaksInLearning = breaksInLearning
                });
            }
        }

        return englishAndMathsCourses;
    }

    private static decimal? ResolveCombinedFundingAdjustmentPercentage(decimal? priorLearningAdjustment, decimal? otherFundingAdjustment)
    {
        var priorLearningHadNonZeroAdjustment = priorLearningAdjustment.HasValue && priorLearningAdjustment.Value != 0;
        var otherFundingHadNonZeroAdjustment = otherFundingAdjustment.HasValue && otherFundingAdjustment.Value != 0;

        if (!priorLearningHadNonZeroAdjustment && !otherFundingHadNonZeroAdjustment)
        {
            return null;
        }

        if(priorLearningHadNonZeroAdjustment && !otherFundingHadNonZeroAdjustment) {
            return priorLearningAdjustment!.Value / 100;
        }

        if(!priorLearningHadNonZeroAdjustment && otherFundingHadNonZeroAdjustment) {
            return otherFundingAdjustment!.Value / 100;
        }

        return (priorLearningAdjustment!.Value / 100) * (otherFundingAdjustment!.Value / 100);
    }
}
