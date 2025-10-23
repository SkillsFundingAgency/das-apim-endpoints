using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;
using SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.Fm36;

public class GetFm36QueryHandler : IRequestHandler<GetFm36Query, GetFm36Result>
{
    const int SimplificationEarningsPlatform = 2;

    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;
    private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;
    private readonly ILogger<GetFm36QueryHandler> _logger;

    public GetFm36QueryHandler(ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient,
        ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient,
        ILogger<GetFm36QueryHandler> logger)
    {
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
        _collectionCalendarApiClient = collectionCalendarApiClient;
        _logger = logger;
    }

    public async Task<GetFm36Result> Handle(GetFm36Query request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllEarningsQuery for provider {ukprn}", request.Ukprn);

        var currentAcademicYear = await GetCurrentAcademicYear(request);
        var (learnings, totalLearners) = await GetLearnings(request);
        var earnings = await GetRelatedEarnings(request, learnings);
        var joinedApprenticeships = JoinLearningAndEarningData(learnings, earnings, currentAcademicYear);

        var fm36Learners = TransformToFm36Learners(joinedApprenticeships, currentAcademicYear, request.CollectionPeriod);

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

    private async Task<(List<Learning>, int)> GetLearnings(GetFm36Query request)
    {
        List<Learning>? learners = null;
        int totalItems = 0;

        var innerRequest = new GetLearningsRequest { 
            Ukprn = request.Ukprn, 
            CollectionYear = request.CollectionYear, 
            CollectionPeriod = request.CollectionPeriod 
        };

        if(request.IsPaged)
        {
            innerRequest.Page = request.Page;
            innerRequest.PageSize = request.PageSize;
            var pagedlearners = await _learningApiClient.Get<GetPagedLearnersFromLearningInner>(innerRequest);
            if(pagedlearners != null)
            {
                learners = pagedlearners.Items;
                totalItems = pagedlearners.TotalItems;
            }

        }
        else
        {
            learners = await _learningApiClient.Get<List<Learning>>(innerRequest);
            totalItems = learners.Count;
        }

        
        if (learners == null || !learners.Any())
        {
            learners = new List<Learning>();
            if(request.IsPaged)
                _logger.LogWarning("No learning data returned for {ukprn} from Learnings Paged Inner {pageNumber} {pageSize}", request.Ukprn, request.Page, request.PageSize);
            else
                _logger.LogWarning("No learning data returned for {ukprn} from Learnings Inner", request.Ukprn);
        }

        return (learners!, totalItems);
    }

    private async Task<List<Apprenticeship>> GetRelatedEarnings(GetFm36Query request, List<Learning> learnings)
    {
        var learningKeys = request.IsPaged ? learnings.Select(x => x.Key).ToList() : new List<Guid>(); // only send keys if paged
        var earningsRequest = new PostGetFm36DataRequest(request.Ukprn, request.CollectionYear, request.CollectionPeriod, learningKeys);
        var result = await _earningsApiClient.PostWithResponseCode<GetFm36DataResponse>(earningsRequest);

        if (!result.StatusCode.IsSuccessStatusCode() || result.Body == null)
        {
            _logger.LogWarning("No earnings data returned for {ukprn} from Earnings Inner", request.Ukprn);
            return new List<Apprenticeship>();
        }

        return result.Body.Apprenticeships;
    }

    private List<JoinedEarningsApprenticeship> JoinLearningAndEarningData(List<Learning> learnings, List<Apprenticeship> earnings, GetAcademicYearsResponse currentAcademicYear)
    {
        var joinedApprenticeships = new List<JoinedEarningsApprenticeship>();

        foreach (var learning in learnings)
        {
            // Find matching entries in earningsData
            var matchingEarnings = earnings.FirstOrDefault(e => e.Key == learning.Key);

            if (matchingEarnings != null)
            {
                _logger.LogInformation($"Processing learning with key: {learning.Key}");
                joinedApprenticeships.Add(new JoinedEarningsApprenticeship(learning, matchingEarnings, currentAcademicYear.GetShortAcademicYear()));
            }
            else
            {
                _logger.LogWarning($"No matching earnings data found for learning with key: {learning.Key}");
                throw new InvalidOperationException($"Earnings data missing for learning key: {learning.Key}");
            }
        }

        return joinedApprenticeships;
    }


    private List<FM36Learner> TransformToFm36Learners(
        List<JoinedEarningsApprenticeship> joinedApprenticeships,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod)
    {
        var learners = new List<FM36Learner>();

        var result = joinedApprenticeships
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
                    _logger.LogError(ex, "Error processing apprenticeship with key: {Key}", joinedApprenticeship.Key);
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
