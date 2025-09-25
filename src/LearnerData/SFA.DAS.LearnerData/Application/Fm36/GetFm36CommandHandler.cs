using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;
using SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.Fm36;

public class GetFm36CommandHandler : IRequestHandler<GetFm36Command, GetFm36Result>
{
    const int SimplificationEarningsPlatform = 2;

    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;
    private readonly ILogger<GetFm36CommandHandler> _logger;

    public GetFm36CommandHandler(ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient,
        ILogger<GetFm36CommandHandler> logger)
    {
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
        _logger = logger;
    }

    public async Task<GetFm36Result> Handle(GetFm36Command request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEarningsQuery for provider {ukprn}", request.Ukprn);

        var learningDataTask = _learningApiClient.Get<GetLearningsResponse>(new GetLearningsRequest { Ukprn = request.Ukprn, CollectionYear = request.CollectionYear, CollectionPeriod = request.CollectionPeriod });
        var earningsDataTask = _earningsApiClient.Get<GetFm36DataResponse>(new GetFm36DataRequest(request.Ukprn, request.CollectionYear, request.CollectionPeriod));
        var currentAcademicYearTask = _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByYearRequest(request.CollectionYear));

        await Task.WhenAll(learningDataTask, earningsDataTask, currentAcademicYearTask);

        var learningData = learningDataTask.Result;
        var earningsData = earningsDataTask.Result;
        var currentAcademicYear = currentAcademicYearTask.Result;

        if (currentAcademicYear == null)
        {
            _logger.LogWarning($"Collection Calendar Api failed to return data on requested collection year {request.CollectionYear}. A calculated fall-back will be used instead.");
            currentAcademicYear = AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(request.CollectionYear);
        }

        if (!IsDataReturnedValid(request.Ukprn, learningData, earningsData))
            return new GetFm36Result { FM36Learners = [] };

        _logger.LogInformation("Found {learningsCount} learnings, {earningsApprenticeshipsCount} earnings apprenticeships, for provider {ukprn}", learningData.Learnings.Count, earningsData.Count, request.Ukprn);
        _logger.LogInformation($"Academic year {currentAcademicYear.AcademicYear}: {currentAcademicYear.StartDate:yyyy-MM-dd} - {currentAcademicYear.EndDate:yyyy-MM-dd}");

        var joinedApprenticeships = new List<JoinedEarningsApprenticeship>();

        foreach (var learning in learningData.Learnings)
        {
            // Find matching entries in earningsData
            var matchingEarnings = earningsData.FirstOrDefault(e => e.Key == learning.Key);

            if (matchingEarnings != null)
            {
                _logger.LogInformation($"Processing learning with key: {learning.Key}");
                joinedApprenticeships.Add(new JoinedEarningsApprenticeship(learning, matchingEarnings, currentAcademicYear.GetShortAcademicYear()));
            }
        }

        var result = new GetFm36Result
        {
            FM36Learners = TransformToFm36Learners(joinedApprenticeships, currentAcademicYear, request.CollectionPeriod)
        };

        return result;
    }

    private FM36Learner[] TransformToFm36Learners(
        List<JoinedEarningsApprenticeship> joinedApprenticeships,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod)
    {
        FM36Learner[] result = joinedApprenticeships
            .Select(joinedApprenticeship =>
            {
                try
                {
                    return new FM36Learner
                    {
                        ULN = long.Parse(joinedApprenticeship.Uln),
                        LearnRefNumber = EarningsFM36Constants.LearnRefNumber,
                        EarningsPlatform = SimplificationEarningsPlatform,
                        PriceEpisodes = PriceEpisodeBuilder.GetPriceEpisodes(joinedApprenticeship, currentAcademicYear, collectionPeriod),
                        LearningDeliveries = LearningDeliveryBuilder.GetLearningDeliveries(currentAcademicYear, joinedApprenticeship),
                        HistoricEarningOutputValues = new List<HistoricEarningOutputValues>()
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing apprenticeship with key: {Key}", joinedApprenticeship.Uln);
                    return null;
                }

            })
            .Where(learner => learner != null)
            .ToArray()!;

        return result;
    }


    private bool IsDataReturnedValid(long ukprn, GetLearningsResponse learningsData, GetFm36DataResponse earningsData)
    {
        if (learningsData == null || learningsData.Learnings == null || !learningsData.Learnings.Any())
        {
            _logger.LogWarning("No apprenticeships data returned for {ukprn}", ukprn);
            return false;
        }

        if (earningsData == null || !earningsData.Any())
        {
            _logger.LogWarning("No earnings data returned for {ukprn}", ukprn);
            return false;
        }

        return true;
    }


}
