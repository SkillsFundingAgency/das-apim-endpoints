using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
public class GetLiveVacanciesApiRequest : IGetApiRequest
{
    public GetLiveVacanciesApiRequest(int? pageNumber, int? pageSize, DateTime? closingDate)
    {
        var getUrl = $"api/vacancies/live?pageSize={pageSize}&page={pageNumber}";

        if (closingDate != null)
        {
            getUrl += $"&closingDate={closingDate.Value.Date}";
        }
        
        GetUrl = getUrl;
    }

    public string GetUrl { get; }
}
