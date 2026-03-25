using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetShortCourseLearningsForEarnings : IGetApiRequest
{
    public long Ukprn { get; set; }
    public int CollectionYear { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public string GetUrl
    {
        get
        {
            var url = $"{Ukprn}/{CollectionYear}/shortCourses";

            if (Page.HasValue && PageSize.HasValue)
                url += $"?page={Page}&pageSize={PageSize}";

            return url;
        }
    }
}