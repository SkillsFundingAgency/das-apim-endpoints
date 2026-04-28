using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class RecalculateApplicationCostProjectionRequest : IPostApiRequest
{
    public int ApplicationId { get; set; }
    public object Data { get; set; } = null;

    public string PostUrl => $"applications/{ApplicationId}/recalculate-cost-projection";
}