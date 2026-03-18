using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsQueryHandler : IRequestHandler<GetShortCourseEarningsQuery, GetShortCourseEarningsResult>
{
    private readonly ILogger<GetShortCourseEarningsQueryHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
    private readonly IEarningsApiClient<EarningsApiConfiguration> _earningsApiClient;

    public GetShortCourseEarningsQueryHandler(
        ILogger<GetShortCourseEarningsQueryHandler> logger, 
        ILearningApiClient<LearningApiConfiguration> learningApiClient,
        IEarningsApiClient<EarningsApiConfiguration> earningsApiClient)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
        _earningsApiClient = earningsApiClient;
    }

    public async Task<GetShortCourseEarningsResult> Handle(GetShortCourseEarningsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetShortCourseEarningsQuery for provider {ukprn}", request.Ukprn);

        var (learnings, totalLearners) = await GetLearnings(request);

        var earnings = await GetRelatedEarnings(request, learnings);

        var shortCourseEarnings = CombineLearningsAndEarningsData(learnings, earnings);

        return BuildResult(request, shortCourseEarnings, totalLearners);
    }

    private async Task<(List<Learning>, int)> GetLearnings(GetShortCourseEarningsQuery request)
    {
        List<Learning>? learners = null;
        int totalItems = 0;

        var innerRequest = new GetShortCourseLearningsForEarnings
        {
            Ukprn = request.Ukprn,
            CollectionYear = request.CollectionYear
        };

        innerRequest.Page = request.Page;
        innerRequest.PageSize = request.PageSize;
        var pagedlearners = await _learningApiClient.Get<GetPagedLearnersFromLearningInner>(innerRequest);
        if (pagedlearners != null)
        {
            learners = pagedlearners.Items;
            totalItems = pagedlearners.TotalItems;
        }

        if (learners == null || !learners.Any())
        {
            learners = new List<Learning>();
            _logger.LogWarning("No learning data returned for {ukprn} from Learnings Paged Inner {pageNumber} {pageSize}", request.Ukprn, request.Page, request.PageSize);

        }

        return (learners!, totalItems);
    }

    private async Task<List<EarningsByLearningKey>> GetRelatedEarnings(GetShortCourseEarningsQuery request, List<Learning> learnings)
    {

        var tasks = learnings.Select(learning => GetShortCourseEarning(request.Ukprn, learning)).ToList();

        var results = await Task.WhenAll(tasks);

        if (results.All(x=>x.EarningsData != null))
        {
            _logger.LogInformation("All earnings data successfully retrieved for {ukprn} from Earnings API", request.Ukprn);
            return results.ToList();
        }

        throw new ApplicationException($"Failed to retrieve all earnings data for {request.Ukprn} from Earnings API.");
    }

    private async Task<EarningsByLearningKey> GetShortCourseEarning(long ukprn, Learning learning)
    {
        var request = new GetShortCourseDataRequest(ukprn, learning.LearningKey);
        var response = await _earningsApiClient.GetWithResponseCode<GetShortCourseDataResponse>(request);

        if (response.StatusCode.IsSuccessStatusCode())
        {
            _logger.LogInformation("Successfully retrieved earnings data for {learningKy} from Earnings API", learning.LearningKey);
            return new EarningsByLearningKey(learning.LearningKey, response.Body);
        }

        _logger.LogError("Failed to retrieve earnings data for {learningKey} from Earnings API. Status code: {statusCode}", learning.LearningKey, response.StatusCode);
        return new EarningsByLearningKey(learning.LearningKey, null);
    }

    private List<ShortCourseActiveEarnings> CombineLearningsAndEarningsData(List<Learning> learnings, List<EarningsByLearningKey> earnings)
    {
        var combined = new List<ShortCourseActiveEarnings>();

        foreach (var learning in learnings)
        {
            var earning = earnings.Single(e => e.LearningKey == learning.LearningKey);

            combined.Add(new ShortCourseActiveEarnings
            {
                LearningKey = learning.LearningKey.ToString(),
                Courses = learning.Episodes.Select(episode => new Course
                {
                    Approved = episode.IsApproved,
                    CoursePrice = episode.Price,
                    Earnings = earning.EarningsData!.Earnings.Select(e=> new Earning
                    {
                        Amount = e.Amount,
                        CollectionPeriod = e.CollectionPeriod,
                        CollectionYear = e.CollectionYear,
                        Milestone = e.Type
                    }).ToList()
                }).ToList()
            });
        }

        return combined;
    }

    private GetShortCourseEarningsResult BuildResult(GetShortCourseEarningsQuery query, List<ShortCourseActiveEarnings> shortCourseEarnings, int totalItems)
    {
        var result = new GetShortCourseEarningsResult { Items = shortCourseEarnings };

        result.Page = query.Page;
        result.PageSize = query.PageSize!.Value;
        result.TotalItems = totalItems;

        return result;
    }
     
}

public class EarningsByLearningKey
{
    public Guid LearningKey { get; set; }
    public GetShortCourseDataResponse? EarningsData { get; set; }

    public EarningsByLearningKey(Guid learningKey, GetShortCourseDataResponse? earningsData)
    {
        LearningKey = learningKey;
        EarningsData = earningsData;
    }
}