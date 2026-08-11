using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface ICreateUnapprovedApprenticeshipLearningRequestBuilder
{
    Task<PostCreateUnapprovedApprenticeshipLearningRequest> Build(long ukprn, CreateLearnerRequest createLearnerRequest, CreateDraftLearnerApiPutResponse learningApiPutResponse, UpdateLearningRequestBody requestBody);
}

public class CreateUnapprovedApprenticeshipLearningRequestBuilder(ICourseService courseService) : ICreateUnapprovedApprenticeshipLearningRequestBuilder
{
    public async Task<PostCreateUnapprovedApprenticeshipLearningRequest> Build(
        long ukprn,
        CreateLearnerRequest createLearnerRequest,
        CreateDraftLearnerApiPutResponse learningApiPutResponse,
        UpdateLearningRequestBody requestBody)
    {
        var firstOnProgramme = createLearnerRequest.Delivery.OnProgramme
            .OrderBy(x => x.StartDate)
            .First();

        var matchingOnProgrammes = createLearnerRequest.Delivery.OnProgramme
            .Where(x => x.StandardCode == firstOnProgramme.StandardCode && x.AgreementId == firstOnProgramme.AgreementId)
            .OrderBy(x => x.StartDate)
            .ToList();

        var fundingBandMaximum = await GetFundingBandMaximum(firstOnProgramme);

        var request = new CreateUnapprovedApprenticeshipLearningRequest
        {
            LearningKey = learningApiPutResponse.LearningKey,
            EpisodeKey = learningApiPutResponse.LearningEpisodeKey,
            ApprovalsApprenticeshipId = 0,
            Learner = new DraftApprenticeshipLearnerRequest
            {
                DateOfBirth = requestBody.Learner.DateOfBirth,
                Uln = requestBody.Learner.Uln.ToString(),
                Care = new DraftCareRequest
                {
                    HasEHCP = requestBody.Learner.Care.HasEHCP,
                    IsCareLeaver = requestBody.Learner.Care.IsCareLeaver,
                    CareLeaverEmployerConsentGiven = requestBody.Learner.Care.CareLeaverEmployerConsentGiven
                }
            },
            OnProgramme = new DraftApprenticeshipOnProgrammeRequest
            {
                TrainingCode = firstOnProgramme.StandardCode.ToString(),
                Ukprn = ukprn,
                EmployerAccountId = 0,
                FundingEmployerAccountId = null,
                LegalEntityName = string.Empty,
                FundingType = ApprenticeshipFundingType.Levy,
                FundingBandMaximum = fundingBandMaximum
            },
            CompletionDate = requestBody.Learner.CompletionDate,
            WithdrawalDate = requestBody.Delivery.WithdrawalDate,
            PauseDate = requestBody.OnProgramme.PauseDate,
            AchievementDate = requestBody.OnProgramme.AchievementDate,
            Prices = learningApiPutResponse.Prices.Select(x => new LearningEpisodePriceItem
            {
                Key = x.Key,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TrainingPrice = x.TrainingPrice,
                EndPointAssessmentPrice = x.EndPointAssessmentPrice,
                TotalPrice = x.TotalPrice
            }).ToList(),
            PeriodsInLearning = GetPeriodsInLearning(matchingOnProgrammes),
            EnglishAndMaths = GetEnglishAndMaths(requestBody.EnglishAndMathsCourses),
            LearningSupport = requestBody.LearningSupport.Select(x => new ApprenticeshipLearningSupportItem
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList()
        };

        return new PostCreateUnapprovedApprenticeshipLearningRequest(request);
    }

    private async Task<int?> GetFundingBandMaximum(CreateLearnerRequest.OnProgrammeDetails firstOnProgramme)
    {
        var response = await courseService.GetStandardDetailsById(firstOnProgramme.StandardCode.ToString());
        return response.MaxFundingOn(firstOnProgramme.StartDate);
    }

    private static List<ApprenticeshipPeriodInLearningItem> GetPeriodsInLearning(List<CreateLearnerRequest.OnProgrammeDetails> onProgrammes)
    {
        return onProgrammes.Select(onProgramme => new ApprenticeshipPeriodInLearningItem
        {
            StartDate = onProgramme.StartDate,
            EndDate = onProgramme.CompletionDate ?? onProgramme.PauseDate ?? onProgramme.WithdrawalDate,
            OriginalExpectedEndDate = onProgramme.ExpectedEndDate
        }).ToList();
    }

    private static List<DraftEnglishAndMathsItem> GetEnglishAndMaths(List<MathsAndEnglishDetails> englishAndMathsCourses)
    {
        return englishAndMathsCourses.Select(course => new DraftEnglishAndMathsItem
        {
            StartDate = course.StartDate,
            EndDate = course.PlannedEndDate,
            Course = course.Course,
            LearnAimRef = course.LearnAimRef,
            Amount = course.Amount,
            CombinedFundingAdjustmentPercentage = course.CombinedFundingAdjustmentPercentage,
            PauseDate = course.PauseDate,
            WithdrawalDate = course.WithdrawalDate,
            CompletionDate = course.CompletionDate,
            PeriodsInLearning = GetEnglishAndMathsPeriods(course)
        }).ToList();
    }

    private static List<ApprenticeshipPeriodInLearningItem> GetEnglishAndMathsPeriods(MathsAndEnglishDetails course)
    {
        if (course.BreaksInLearning.Count == 0)
        {
            return
            [
                new ApprenticeshipPeriodInLearningItem
                {
                    StartDate = course.StartDate,
                    EndDate = course.CompletionDate ?? course.PauseDate ?? course.WithdrawalDate,
                    OriginalExpectedEndDate = course.PlannedEndDate
                }
            ];
        }

        var periods = new List<ApprenticeshipPeriodInLearningItem>();

        var periodStart = course.StartDate;
        foreach (var breakInLearning in course.BreaksInLearning.OrderBy(x => x.StartDate))
        {
            periods.Add(new ApprenticeshipPeriodInLearningItem
            {
                StartDate = periodStart,
                EndDate = breakInLearning.StartDate,
                OriginalExpectedEndDate = breakInLearning.PriorPeriodExpectedEndDate
            });

            periodStart = breakInLearning.EndDate;
        }

        periods.Add(new ApprenticeshipPeriodInLearningItem
        {
            StartDate = periodStart,
            EndDate = course.CompletionDate ?? course.PauseDate ?? course.WithdrawalDate,
            OriginalExpectedEndDate = course.PlannedEndDate
        });

        return periods;
    }
}
