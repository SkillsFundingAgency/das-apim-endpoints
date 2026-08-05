using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
public record GetReportsByUkprnRequest(int Ukprn) : IGetApiRequest
{
    public string GetUrl => $"api/reports/{Ukprn}/provider";
}
