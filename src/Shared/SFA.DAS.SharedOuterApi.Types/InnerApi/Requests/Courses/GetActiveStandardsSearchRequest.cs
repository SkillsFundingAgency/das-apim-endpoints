using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Constants;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetActiveStandardsSearchRequest : IGetApiRequest
{
    public List<int> RouteIds { get; set; } = [];
    public List<int> Levels { get; set; } = [];
    public List<LearningType> LearningTypes { get; set; } = [];
    public string Keyword { get; set; } = string.Empty;
    public CoursesOrderBy OrderBy { get; set; } = CoursesOrderBy.Score;

    public string GetUrl => BuildUrl();

    private const string _BASE_URL = "api/courses/search";

    private string BuildUrl()
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(Keyword))
        {
            queryParams.Add($"keyword={Uri.EscapeDataString(Keyword)}");
        }

        if (RouteIds != null && RouteIds.Count > 0)
        {
            foreach (int routeId in RouteIds)
            {
                queryParams.Add($"routeIds={routeId}");
            }
        }

        if (Levels != null && Levels.Count > 0)
        {
            foreach (int level in Levels)
            {
                queryParams.Add($"levels={level}");
            }
        }

        foreach (var learningType in LearningTypes)
        {
            queryParams.Add($"learningTypes={learningType}");
        }

        queryParams.Add("filter=Active");

        queryParams.Add($"orderby={OrderBy}");

        var queryString = string.Join("&", queryParams);

        return $"{_BASE_URL}{(queryString.Length > 0 ? "?" + queryString : string.Empty)}";
    }
}