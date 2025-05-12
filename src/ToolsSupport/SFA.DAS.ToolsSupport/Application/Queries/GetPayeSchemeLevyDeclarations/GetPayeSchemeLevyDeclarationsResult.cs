using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;

public class GetPayeSchemeLevyDeclarationsResult
{
    public PayeScheme PayeScheme { get; set; } = new();
    public PayeLevySubmissionsResponseCodes StatusCode { get; set; }
    public PayeSchemeLevyDeclarations LevySubmissions { get; set; } = new();
}
public enum PayeLevySubmissionsResponseCodes
{
    Success,
    AccountNotFound,
    UnexpectedError
}