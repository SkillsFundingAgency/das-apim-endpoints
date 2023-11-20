using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
public class GetLiveVacanciesApiRequest : IGetApiRequest
{
    private readonly int? _pageNumber;
    private readonly int? _pageSize;

    public GetLiveVacanciesApiRequest(int? pageNumber, int? pageSize)
    {
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public string GetUrl => $"api/livevacancies?pageSize={_pageSize}&pageNo={_pageNumber}";
}
