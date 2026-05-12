using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

public class GetSearchIndexStatisticsRequest : IGetApiRequest
{
    public string Version => "2.0";
    public string GetUrl => "api/vacancies/statistics";
}