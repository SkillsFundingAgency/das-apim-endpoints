using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Assessor;

public class GetEndpointAssessmentsRequest(int ukprn) : IGetApiRequest
{
    public int Ukprn { get; } = ukprn;
    public string GetUrl => $"api/ao/assessments?ukprn={Ukprn}";
}