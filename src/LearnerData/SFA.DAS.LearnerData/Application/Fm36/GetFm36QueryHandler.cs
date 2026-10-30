using DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;
using SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.Fm36;

public class GetFm36QueryHandler : IRequestHandler<GetFm36Query, GetFm36Result>
{
    const int SimplificationEarningsPlatform = 2;

    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;
    private readonly ILearnerDataCacheService _distributedCache;
    private readonly ILogger<GetFm36QueryHandler> _logger;

    public GetFm36QueryHandler(ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient,
        ILearnerDataCacheService distributedCache,
        ILogger<GetFm36QueryHandler> logger)
    {
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<GetFm36Result> Handle(GetFm36Query request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEarningsQuery for provider {ukprn}", request.Ukprn);

        var currentAcademicYear = await GetCurrentAcademicYear(request);

        var (earnings, totalLearners) = await GetEarnings(request);

        var learnings = await GetLearnings(request, earnings);

        var sldLearners = await _distributedCache.GetLearners<UpdateLearnerRequest>(request.Ukprn, learnings.Select(x => x.Uln), cancellationToken);

        var joinedLearnerData = JoinDataSources(learnings, earnings, sldLearners, currentAcademicYear);

        var fm36Learners = TransformToFm36Learners(joinedLearnerData, currentAcademicYear, request.CollectionPeriod, request.Ukprn);

        return BuildResult(request, fm36Learners, totalLearners);
    }

    private async Task<GetAcademicYearsResponse> GetCurrentAcademicYear(GetFm36Query request)
    {
        var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByYearRequest(request.CollectionYear));
        if (currentAcademicYear == null)
        {
            _logger.LogWarning($"Collection Calendar Api failed to return data on requested collection year {request.CollectionYear}. A calculated fall-back will be used instead.");
            currentAcademicYear = AcademicYearFallbackHelper.GetFallbackAcademicYearResponse(request.CollectionYear);
        }

        return currentAcademicYear;
    }

    private async Task<(List<Apprenticeship>, int)> GetEarnings(GetFm36Query request)
    {
        var earningsRequest = request.IsPaged
            ? new PostGetFm36DataRequest(request.Ukprn, request.CollectionYear, request.CollectionPeriod, request.Page, request.PageSize)
            : new PostGetFm36DataRequest(request.Ukprn, request.CollectionYear, request.CollectionPeriod);

        var result = await _earningsApiClient.PostWithResponseCode<GetFm36DataResponse>(earningsRequest);

        if (!result.StatusCode.IsSuccessStatusCode() || result.Body == null)
        {
            _logger.LogWarning("No earnings data returned for {ukprn} from Earnings Inner", request.Ukprn);
            return (new List<Apprenticeship>(), 0);
        }

        var apprenticeships = result.Body.Apprenticeships ?? [];

        if (!apprenticeships.Any())
        {
            _logger.LogWarning("No earnings data returned for {ukprn} from Earnings Inner {pageNumber} {pageSize}", request.Ukprn, request.Page, request.PageSize);
        }

        return (apprenticeships, result.Body.TotalItems);
    }

    private async Task<List<Learning>> GetLearnings(GetFm36Query request, List<Apprenticeship> earnings)
    {
        if (!earnings.Any())
        {
            return new List<Learning>();
        }

        var learningKeys = earnings.Select(x => x.Key).ToList();
        var result = await _learningApiClient.PostWithResponseCode<List<Learning>>(new PostGetLearningsRequest(request.Ukprn, learningKeys));

        if (!result.StatusCode.IsSuccessStatusCode() || result.Body == null)
        {
            _logger.LogWarning("No learning data returned for {ukprn} from Learnings Inner for {keyCount} learning keys", request.Ukprn, learningKeys.Count);
            return new List<Learning>();
        }

        return result.Body;
    }

    private List<JoinedLearnerData> JoinDataSources(List<Learning> learnings, List<Apprenticeship> earnings, List<UpdateLearnerRequest> sldData, GetAcademicYearsResponse currentAcademicYear)
    {
        var joinedApprenticeships = new List<JoinedLearnerData>();

        foreach (var earning in earnings)
        {
            var matchingLearning = learnings.FirstOrDefault(l => l.Key == earning.Key);

            if (matchingLearning == null)
            {
                _logger.LogWarning($"No matching learning data found for earnings with key: {earning.Key}");
                throw new InvalidOperationException($"Learning data missing for earnings key: {earning.Key}");
            }

            var matchingSld = sldData.FirstOrDefault(s => s.Learner.Uln.ToString() == matchingLearning.Uln);

            if (matchingSld == null)
            {
                _logger.LogWarning($"No matching SLD data found for learning with key: {matchingLearning.Key}");
                continue;
            }

            _logger.LogInformation($"Processing learning with key: {matchingLearning.Key}");

            joinedApprenticeships.Add(new JoinedLearnerData(matchingLearning, earning, matchingSld, currentAcademicYear.GetShortAcademicYear()));
        }

        return joinedApprenticeships;
    }

    private List<FM36Learner> TransformToFm36Learners(
        List<JoinedLearnerData> joinedLearnerData,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod,
        long ukprn)
    {
        var learners = new List<FM36Learner>();

        var result = joinedLearnerData
            .Select(joinedLearner =>
            {
                try
                {
                    return new FM36Learner
                    {
                        ULN = long.Parse(joinedLearner.Uln),
                        LearnRefNumber = ukprn.ToString(),
                        EarningsPlatform = SimplificationEarningsPlatform,
                        PriceEpisodes = PriceEpisodeBuilder.GetPriceEpisodes(joinedLearner, currentAcademicYear, collectionPeriod),
                        LearningDeliveries = LearningDeliveryBuilder.GetLearningDeliveries(currentAcademicYear, joinedLearner),
                        HistoricEarningOutputValues = new List<HistoricEarningOutputValues>()
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing apprenticeship with key: {Key}", joinedLearner.Key);
                    return null;
                }

            })
            .Where(learner => learner != null)
            .ToList()!;

        if(result != null && result.Any())
            learners.AddRange(result!);

        return learners;
    }

    private GetFm36Result BuildResult(GetFm36Query query, List<FM36Learner> fm36Learners, int totalItems)
    {
        var result = new GetFm36Result { Items = fm36Learners };
        
        if(query.IsPaged)
        {
            result.Page = query.Page;
            result.PageSize = query.PageSize!.Value;
            result.TotalItems = totalItems;
        }

        return result;
    }
}
