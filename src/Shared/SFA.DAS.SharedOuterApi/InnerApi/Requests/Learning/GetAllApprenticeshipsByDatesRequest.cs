using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
public class GetAllApprenticeshipsByDatesRequest(string ukprn, string startDate, string endDate, int page, int? pageSize = 20) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/apprenticeships/by-dates?startDate={startDate}&endDate={endDate}&page={page}&pageSize={pageSize}";
}

