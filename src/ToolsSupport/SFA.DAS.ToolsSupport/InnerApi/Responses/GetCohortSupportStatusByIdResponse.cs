using SFA.DAS.SharedOuterApi.InnerApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetCohortSupportStatusByIdResponse
{
    public long CohortId { get; set; }
    public int NoOfApprentices { get; set; }
    public string CohortStatus { get; set; }
}