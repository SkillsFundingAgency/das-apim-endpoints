using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public class GetRolloverCandidatesApiRequest : IGetApiRequest
{
    public string GetUrl => "api/rollover/rollovercandidates";
}