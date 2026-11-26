using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

/// <summary>
/// Gets fm36 data for a provider. Only learnings that are opted in to the new earnings calculation (DAS funding platform) are returned.
/// If a page and page size are supplied the results are paged, and the response contains the total number of matching learnings.
/// </summary>
/// <remarks>Although this is a Post, no data is added or modified in the earnings inner domain</remarks>
public class PostGetFm36DataRequest : IPostApiRequest
{
    public long Ukprn { get; }
    public int CollectionYear { get; set; }
    public byte CollectionPeriod { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public string PostUrl
    {
        get
        {
            var url = $"{Ukprn}/fm36/{CollectionYear}/{CollectionPeriod}";

            if (Page.HasValue && PageSize.HasValue)
                url += $"?page={Page}&pageSize={PageSize}";

            return url;
        }
    }

    public object Data { get; set; }

    public PostGetFm36DataRequest(long ukprn, int collectionYear, byte collectionPeriod, int? page = null, int? pageSize = null)
    {
        Ukprn = ukprn;
        CollectionYear = collectionYear;
        CollectionPeriod = collectionPeriod;
        Page = page;
        PageSize = pageSize;
        Data = new List<Guid>(); // earnings inner decides which learnings to return
    }
}
