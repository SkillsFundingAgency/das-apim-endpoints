using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LearnerData.Services;

public interface IUpdateLearningRequestBodyBuilder
{
    UpdateLearningRequestBody Build(long ukprn, UpdateLearnerRequest updateLearnerRequest);
    UpdateLearningRequestBody Build(long ukprn, CreateLearnerRequest createLearnerRequest);
}

public class UpdateLearningRequestBodyBuilder(
    ILearningSupportService learningSupportService,
    IBreaksInLearningService breaksInLearningService,
    ICostsService costsService)
    : IUpdateLearningRequestBodyBuilder
{
    public UpdateLearningRequestBody Build(long ukprn, UpdateLearnerRequest updateLearnerRequest)
    {
        return BuildInternal(
            ukprn,
            updateLearnerRequest.Learner,
            updateLearnerRequest.Delivery.EnglishAndMaths,
            updateLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(),
            updateLearnerRequest.EnglishAndMathsLearningSupport());
    }

    public UpdateLearningRequestBody Build(long ukprn, CreateLearnerRequest createLearnerRequest)
    {
        var baseLearner = new LearnerRequestDetails
        {
            FirstName = createLearnerRequest.Learner.FirstName,
            LastName = createLearnerRequest.Learner.LastName,
            Email = createLearnerRequest.Learner.Email,
            Dob = createLearnerRequest.Learner.Dob ?? DateTime.MinValue,
            HasEhcp = createLearnerRequest.Learner.HasEhcp ?? false,
            Uln = createLearnerRequest.Learner.Uln
        };

        return BuildInternal(
            ukprn,
            baseLearner,
            createLearnerRequest.Delivery.EnglishAndMaths?.Cast<MathsAndEnglish>().ToList(),
            createLearnerRequest.Delivery.OnProgramme.Cast<OnProgrammeRequestDetails>().ToList(),
            createLearnerRequest.EnglishAndMathsLearningSupport());
    }

    private UpdateLearningRequestBody BuildInternal(
        long ukprn,
        LearnerRequestDetails learner,
        List<MathsAndEnglish> englishAndMaths,
        List<OnProgrammeRequestDetails> onProgramme,
        List<KeyValuePair<string, List<LearningSupport>>> englishAndMathsLearningSupport)
    {
        var (firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes) = SelectEpisode(onProgramme);

        var costs = costsService.GetCosts(allMatchingOnProgrammes);
        var onProgrammeDetails = BuildOnProgrammeDetails(firstOnProgramme, latestOnProgramme, allMatchingOnProgrammes, costs);
        var englishAndMathsCourses = BuildEnglishAndMathsDetails(englishAndMaths ?? new List<MathsAndEnglish>());

        //Determine the effective end date of the latest OnProgramme
        var onProgrammeEndDate = new[]
        {
            latestOnProgramme.ExpectedEndDate,
            latestOnProgramme.CompletionDate ?? DateTime.MaxValue,
            latestOnProgramme.WithdrawalDate ?? DateTime.MaxValue,
            latestOnProgramme.PauseDate ?? DateTime.MaxValue
        }.Min();

        var learningSupport = learningSupportService.GetCombinedLearningSupport(
            allMatchingOnProgrammes,
            onProgrammeEndDate,
            onProgrammeDetails.BreaksInLearning,
            englishAndMathsCourses,
            englishAndMathsLearningSupport);

        return new UpdateLearningRequestBody
        {
            Delivery = new Delivery
            {
                WithdrawalDate = latestOnProgramme.WithdrawalDate
            },
            Learner = new LearningUpdateDetails
            {
                FirstName = learner.FirstName,
                LastName = learner.LastName,
                EmailAddress = learner.Email,
                CompletionDate = latestOnProgramme.CompletionDate,
                DateOfBirth = learner.Dob,
                Uln = learner.Uln,
                Care = new CareDetails
                {
                    HasEHCP = learner.HasEhcp,
                    IsCareLeaver = latestOnProgramme.Care.Careleaver,
                    CareLeaverEmployerConsentGiven = latestOnProgramme.Care.EmployerConsent
                }
            },
            OnProgramme = onProgrammeDetails,
            EnglishAndMathsCourses = englishAndMathsCourses,
            LearningSupport = learningSupport
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

    private List<MathsAndEnglishDetails> BuildEnglishAndMathsDetails(List<MathsAndEnglish> mathsAndEnglish)
    {
        var groupedCourses = mathsAndEnglish.GroupBy(c => c.LearnAimRef);

        return groupedCourses.Select(g =>
        {
            if(g.Count() == 1)
            {
                var course = g.First();
                return new MathsAndEnglishDetails
                {
                    Amount = course.Amount,
                    CompletionDate = course.CompletionDate,
                    LearnAimRef = course.LearnAimRef,
                    Course = course.Course,
                    PlannedEndDate = course.EndDate,
                    CombinedFundingAdjustmentPercentage = ResolveCombinedFundingAdjustmentPercentage(course.PriorLearningAdjustment,course.OtherFundingAdjustment),
                    StartDate = course.StartDate,
                    WithdrawalDate = course.WithdrawalDate,
                    PauseDate = course.PauseDate,
                    BreaksInLearning = new List<BreakInLearning>()
                };
            }
            else
            {
                var breaksInLearning = breaksInLearningService.CalculateEnglishAndMathsBreaksInLearning(g.ToList());
                var orderedCourses = g.OrderBy(c => c.StartDate).ToList();
                var firstCourse = orderedCourses.First();
                var latestCourse = orderedCourses.Last();

                return new MathsAndEnglishDetails
                {
                    Amount = latestCourse.Amount,
                    CompletionDate = latestCourse.CompletionDate,
                    LearnAimRef = latestCourse.LearnAimRef,
                    Course = latestCourse.Course,
                    PlannedEndDate = latestCourse.EndDate,
                    CombinedFundingAdjustmentPercentage = ResolveCombinedFundingAdjustmentPercentage(latestCourse.PriorLearningAdjustment,latestCourse.OtherFundingAdjustment),
                    StartDate = firstCourse.StartDate,
                    WithdrawalDate = latestCourse.WithdrawalDate,
                    PauseDate = latestCourse.PauseDate,
                    BreaksInLearning = breaksInLearning
                };
            }
        }).ToList();
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
