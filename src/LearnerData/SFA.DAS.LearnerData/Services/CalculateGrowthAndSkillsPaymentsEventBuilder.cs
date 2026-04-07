using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Services;

public interface ICalculateGrowthAndSkillsPaymentsEventBuilder
{
    public Task<CalculateGrowthAndSkillsPayments> Build(long ukprn,
        UpdateShortCourseLearningPutResponse learningResponse,
        ShortCourseEarningsResponse earningsResponse);
}

public class CalculateGrowthAndSkillsPaymentsEventBuilder : ICalculateGrowthAndSkillsPaymentsEventBuilder
{
    private readonly ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder> _logger;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public CalculateGrowthAndSkillsPaymentsEventBuilder(
        ILogger<CalculateGrowthAndSkillsPaymentsEventBuilder> logger,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
    {
        _logger = logger;
        _collectionCalendarApiClient = collectionCalendarApiClient;
    }

    public async Task<CalculateGrowthAndSkillsPayments> Build(
        long ukprn, UpdateShortCourseLearningPutResponse learningResponse, ShortCourseEarningsResponse earningsResponse)
    {
        var episode = learningResponse.Episodes.Single(); // At time of writing, Short Courses are expected to only have one episode.

        var earnings = await BuildEarnings(learningResponse, earningsResponse, episode);

        return new CalculateGrowthAndSkillsPayments
        {
            EarningsId = earningsResponse.EarningProfileVersion,
            UKPRN = ukprn,
            Learner = new Payments.EarningEvents.Messages.External.Learner
            {
                LearnerKey = learningResponse.LearningKey, // potentially not required
                ULN = long.Parse(learningResponse.Learner.Uln),
                Reference = episode.LearnerRef
            },
            Training = new Training
            {
                LearningKey = learningResponse.LearningKey,
                CourseType = Payments.EarningEvents.Messages.External.CourseType.ShortCourse,
                LearningType = Enum.Parse<LearningType>(episode.LearningType),
                CourseCode = episode.CourseCode, //this is trainingcode in Learning Domain
                CourseReference = episode.CourseCode, //this is also trainingcode in Learning Domain
                AgeAtStartOfTraining = (byte)episode.AgeAtStart,
                StartDate = episode.StartDate,
                PlannedEndDate = episode.PlannedEndDate,
                ActualEndDate = GetActualEndDate(episode.WithdrawalDate, learningResponse.CompletionDate),
                TrainingStatus = GetTrainingStatus(episode.WithdrawalDate, learningResponse.CompletionDate)
            },
            EmployerContribution = 0,
            Earnings = earnings
        };
    }

    private DateTime? GetActualEndDate(DateTime? withdrawalDate, DateTime? completionDate)
    {
        if (withdrawalDate != null)
            return withdrawalDate;
        if (completionDate != null)
            return completionDate;
        return null;
    }

    private TrainingStatus GetTrainingStatus(DateTime? withdrawalDate, DateTime? completionDate)
    {
        if (withdrawalDate != null)
            return TrainingStatus.Withdrawn;

        if(completionDate != null)
            return TrainingStatus.Completed;

        return TrainingStatus.Continuing;
    }

    private async Task<IEnumerable<Earnings>> BuildEarnings(
        UpdateShortCourseLearningPutResponse learningResponse,
        ShortCourseEarningsResponse earningsResponse,
        UpdateShortCourseResultEpisode episode)
    {
        var employerType = Enum.Parse<EmployerType>(episode.EmployerType);

        var earnings = earningsResponse.Instalments
            .GroupBy(i => i.CollectionYear)
            .Select(g => new SFA.DAS.Payments.EarningEvents.Messages.External.Earnings
            {
                AcademicYear = g.Key,
                PricePeriods = new List<PricePeriod> {
                    new PricePeriod
                    {
                        Price = episode.Price,
                        StartDate = episode.StartDate, // This will be adjusted if there are multiple years
                        EndDate = episode.PlannedEndDate, // This will be adjusted if there are multiple years
                        Periods = g
                        .Where(x => x.IsPayable).Select(instalment => new EarningPeriod
                        {
                            EarningType = GetEarningType(instalment),
                            DeliveryPeriod = instalment.CollectionPeriod,
                            Amount = instalment.Amount,
                            Employer = new Employer
                            {
                                AccountId = episode.EmployerAccountId,
                                FundingAccountId = episode.EmployerAccountId,
                                EmployerType = employerType
                            }
                        }).ToList()
                    }
                }
            })
            .OrderByYearPeriod()
            .ToList();

        if(earnings.Count() > 1)
            await SetStartEndDatesForMultipleYears(earnings);

        return earnings;
    }

    /// <summary>
    /// Adjusts price period boundaries when earnings span multiple academic years.
    /// First year keeps its original start date, last year keeps its original end date;
    /// intermediate years use academic year start and end dates.
    /// </summary>
    private async Task SetStartEndDatesForMultipleYears(List<Earnings> earnings)
    {
        var totalEarnings = earnings.Count();

        for (var i = 0; i < totalEarnings; i++)
        {
            var earningYear = earnings[i];
            var academicYear = await GetAcademicYear(earningYear.AcademicYear);

            var pricePeriod = earningYear.PricePeriods.Single();

            if (i > 0) // if this is not the first earning, set the start date to the academic year start date
            {
                pricePeriod.StartDate = academicYear.StartDate;
            }

            if (i < totalEarnings - 1) // if this is not the last earning, set the end date to the academic year end date
            {
                pricePeriod.EndDate = academicYear.EndDate;
            }
        }
    }

    private async Task<GetAcademicYearsResponse> GetAcademicYear(int year)
    {
        var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByYearRequest(year));
        if (currentAcademicYear == null)
        {
            _logger.LogWarning($"Collection Calendar Api failed to return data on requested collection year {year}. A calculated fall-back will be used instead.");
            currentAcademicYear = AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(year);
        }

        return currentAcademicYear;
    }

    private static EarningType GetEarningType(ShortCourseInstalment instalment)
    {
        if(instalment.Type == Milestone.ThirtyPercentLearningComplete.ToString())
            return EarningType.Milestone1;

        if(instalment.Type == Milestone.LearningComplete.ToString())
            return EarningType.Completion;

        throw new ArgumentException($"Unknown instalment type: {instalment.Type}");
    }
}

internal static class EarningsLinqExtensions
{
    public static IEnumerable<Earnings> OrderByYearPeriod(
        this IEnumerable<Earnings> source)
    {
        return source
            .OrderBy(e => e.AcademicYear)
            .Select(e =>
            {
                var academicYear = e.AcademicYear;

                foreach (var pp in e.PricePeriods)
                {
                    pp.Periods = pp.Periods
                        .OrderBy(p => academicYear.GetDateTime(p.DeliveryPeriod))
                        .ToList();
                }

                return e;
            });
    }
}