using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Requests.EarningsInner;
using SFA.DAS.LearnerData.Requests.LearningInner;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Fm99ShortCourseLearning = SFA.DAS.LearnerData.Responses.LearningInner.GetShortCourseLearnersForEarningsResponse.Learning;

namespace SFA.DAS.LearnerData.Application.GetShortCourseEarnings;

public class GetShortCourseEarningsQueryHandler : IRequestHandler<GetShortCourseEarningsQuery, GetShortCourseEarningsQueryResult>
{
    private const string LevyFundingLineType = "GSO Short Courses (Apprenticeship Units) Levy";
    private const string NonLevyFundingLineType = "GSO Short Courses (Apprenticeship Units) Non-Levy";

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

    public async Task<GetShortCourseEarningsQueryResult> Handle(GetShortCourseEarningsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetShortCourseEarningsQuery for provider {ukprn}", request.Ukprn);

        var (learnings, totalLearners) = await GetLearnings(request);

        var earningsByKey = await GetEarningsByKey(request, learnings);

        return BuildResponse(request, learnings, earningsByKey, totalLearners);
    }

    private async Task<(List<Fm99ShortCourseLearning>, int)> GetLearnings(GetShortCourseEarningsQuery request)
    {
        var innerRequest = new GetShortCourseLearningsForEarnings
        {
            Ukprn = request.Ukprn,
            CollectionYear = request.CollectionYear,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var paged = await _learningApiClient.Get<GetPagedShortCourseLearnersResponse>(innerRequest);

        if (paged?.Items == null || !paged.Items.Any())
        {
            _logger.LogWarning("No learning data returned for {ukprn} from Learnings Paged Inner {pageNumber} {pageSize}", request.Ukprn, request.Page, request.PageSize);
            return ([], 0);
        }

        return (paged.Items, paged.TotalItems);
    }

    private async Task<Dictionary<Guid, GetFm99ShortCourseDataResponse>> GetEarningsByKey(GetShortCourseEarningsQuery request, List<Fm99ShortCourseLearning> learnings)
    {
        var tasks = learnings.Select(async learning =>
        {
            var response = await _earningsApiClient.GetWithResponseCode<GetFm99ShortCourseDataResponse>(
                new GetFm99ShortCourseDataRequest(request.Ukprn, learning.LearningKey));

            if (!response.StatusCode.IsSuccessStatusCode())
            {
                _logger.LogError("Failed to retrieve earnings data for {learningKey} from Earnings API. Status code: {statusCode}", learning.LearningKey, response.StatusCode);
                throw new ApplicationException($"Failed to retrieve all earnings data for {request.Ukprn} from Earnings API.");
            }

            _logger.LogInformation("Successfully retrieved earnings data for {learningKey} from Earnings API", learning.LearningKey);
            return (learning.LearningKey, response.Body);
        });

        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(x => x.LearningKey, x => x.Body);
    }

    private static GetShortCourseEarningsQueryResult BuildResponse(
        GetShortCourseEarningsQuery query,
        List<Fm99ShortCourseLearning> learnings,
        Dictionary<Guid, GetFm99ShortCourseDataResponse> earningsByKey,
        int totalItems)
    {
        var learnerItems = learnings.Select(learning =>
        {
            var earnings = earningsByKey[learning.LearningKey];

            return new ShortCourseEarningsLearner
            {
                LearningKey = learning.LearningKey.ToString(),
                LearnerRef = learning.Episodes.FirstOrDefault()?.LearnerRef ?? "",
                Courses = learning.Episodes.Select(episode => new ShortCourseEarningsCourse
                {
                    AimSequenceNumber = 1,
                    FundingLineType = episode.EmployerType == EmployerType.Levy
                        ? LevyFundingLineType
                        : NonLevyFundingLineType,
                    CoursePrice = episode.Price,
                    Approved = episode.IsApproved,
                    Earnings = earnings.Earnings.Select(e => new ShortCourseEarningsEarning
                    {
                        CollectionYear = e.CollectionYear,
                        CollectionPeriod = e.CollectionPeriod,
                        Milestone = e.Type,
                        Amount = e.Amount
                    }).ToList()
                }).ToList()
            };
        }).ToList();

        return new GetShortCourseEarningsQueryResult
        {
            Learners = learnerItems,
            Total = totalItems,
            Page = query.Page,
            PageSize = query.PageSize!.Value,
            TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize!.Value)
        };
    }
}
