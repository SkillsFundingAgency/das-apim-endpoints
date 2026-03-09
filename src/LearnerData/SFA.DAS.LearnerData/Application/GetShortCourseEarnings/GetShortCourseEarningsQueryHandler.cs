using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.GetShortCourseLearnersForEarningsResponse;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsQueryHandler : IRequestHandler<GetShortCourseEarningsQuery, GetShortCourseEarningsResult>
{
    private readonly ILogger<GetShortCourseEarningsQueryHandler> _logger;
    private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;

    public GetShortCourseEarningsQueryHandler(
        ILogger<GetShortCourseEarningsQueryHandler> logger, 
        ILearningApiClient<LearningApiConfiguration> learningApiClient)
    {
        _logger = logger;
        _learningApiClient = learningApiClient;
    }

    public async Task<GetShortCourseEarningsResult> Handle(GetShortCourseEarningsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetShortCourseEarningsQuery for provider {ukprn}", request.Ukprn);

        var (learnings, totalLearners) = await GetLearnings(request);

        var shortCourseEarnings = CombineLearningsAndEarningsData(learnings);

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
    
    private List<ShortCourseEarnings> CombineLearningsAndEarningsData(List<Learning> learnings)
    {
        return learnings.Select(learning => new ShortCourseEarnings
        {
            LearningKey = learning.LearningKey.ToString(),
            //LearnerRef = no mapping,
            Courses = learning.Episodes.Select(episode => new Course
            {
                Approved = episode.IsApproved
            }).ToList()
        }).ToList();
    }

    private GetShortCourseEarningsResult BuildResult(GetShortCourseEarningsQuery query, List<ShortCourseEarnings> shortCourseEarnings, int totalItems)
    {
        var result = new GetShortCourseEarningsResult { Items = shortCourseEarnings };

        result.Page = query.Page;
        result.PageSize = query.PageSize!.Value;
        result.TotalItems = totalItems;

        return result;
    }
     
}