using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
public class GetLiveVacanciesApiRequest : IGetApiRequest
{
    public GetLiveVacanciesApiRequest(int? pageNumber, int? pageSize, DateTime? closingDate)
    {
        var getUrl = $"api/livevacancies?pageSize={pageSize}&pageNo={pageNumber}";

        if (closingDate != null)
        {
            getUrl += $"&closingDate={closingDate.Value.Date}";
        }
        
        GetUrl = getUrl;
    }

    public string GetUrl { get; }
}
