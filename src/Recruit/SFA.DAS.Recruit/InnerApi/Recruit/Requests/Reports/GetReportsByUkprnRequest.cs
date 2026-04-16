using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
public record GetReportsByUkprnRequest(int Ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/reports/{Ukprn}/provider";
}
