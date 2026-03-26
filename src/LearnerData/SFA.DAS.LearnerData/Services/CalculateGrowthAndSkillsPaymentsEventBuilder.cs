using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.Payments.EarningEvents.Messages.External;
using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LearnerData;
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
        var earnings = await BuildEarnings(learningResponse, earningsResponse);

        return new CalculateGrowthAndSkillsPayments
        {
            EarningsId = earningsResponse.EarningProfileVersion,
            UKPRN = ukprn,
            Learner = new Payments.EarningEvents.Messages.External.Learner
            {
                LearnerKey = learningResponse.LearningKey, // this will get moved to training and renamed to LearningKey
                ULN = learningResponse.Uln,
                Reference = learningResponse.LearnerRef
            },
            Training = new Payments.EarningEvents.Messages.External.Training
            {
                CourseType = Payments.EarningEvents.Messages.External.CourseType.ShortCourse,
                LearningType = Enum.Parse<LearningType>(learningResponse.LearningType),
                CourseCode = learningResponse.TrainingCode, //this is trainingcode in Learning Domain
                CourseReference = learningResponse.TrainingCode, //this is also trainingcode in Learning Domain
                AgeAtStartOfTraining = learningResponse.AgeAtStart,
                StartDate = learningResponse.StartDate,
                PlannedEndDate = learningResponse.PlannedEndDate,// expected end date in Learning Domain
                ActualEndDate = GetActualEndDate(learningResponse),// calculated from withdrawal or completion // Reminder to do Completion Date
                TrainingStatus = GetTrainingStatus(learningResponse)
            },
            EmployerContribution = 0,
            Earnings = earnings
        };
    }

    private DateTime? GetActualEndDate(UpdateShortCourseLearningPutResponse learningResponse)
    {
        if (learningResponse.WithdrawalDate != null)
            return learningResponse.WithdrawalDate;
        if (learningResponse.CompletionDate != null)
            return learningResponse.CompletionDate;
        return null;
    }

    private TrainingStatus GetTrainingStatus(UpdateShortCourseLearningPutResponse learningResponse)
    {
        if (learningResponse.WithdrawalDate != null)
            return TrainingStatus.Withdrawn;

        if(learningResponse.CompletionDate != null)
            return TrainingStatus.Completed;

        return TrainingStatus.Continuing;
    }

    private async Task<IEnumerable<Earnings>> BuildEarnings(
        UpdateShortCourseLearningPutResponse learningResponse,
        ShortCourseEarningsResponse earningsResponse)
    {
        var earnings = earningsResponse.Instalments
            .GroupBy(i => i.CollectionYear)
            .Select(g => new SFA.DAS.Payments.EarningEvents.Messages.External.Earnings
            {
                AcademicYear = g.Key,
                PricePeriods = new List<PricePeriod> {
                    new PricePeriod
                    {
                        Price = learningResponse.Price,
                        StartDate = learningResponse.StartDate, // This will be adjusted if there are multiple years
                        EndDate = learningResponse.PlannedEndDate, // This will be adjusted if there are multiple years
                        Periods = g
                        .Where(x => x.IsPayable).Select(instalment => new EarningPeriod
                        {
                            EarningType = GetEarningType(instalment),
                            DeliveryPeriod = instalment.CollectionPeriod,
                            Amount = instalment.Amount,
                            Employer = new Employer
                            {
                                AccountId = learningResponse.EmployerAccountId,
                                FundingAccountId = learningResponse.EmployerAccountId,
                                EmployerType = EmployerType.Levy //TODO: To be set in later story
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
        if(instalment.Type == "ThirtyPercentLearningComplete")
            return EarningType.Milestone1;

        if(instalment.Type == "LearningComplete")
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