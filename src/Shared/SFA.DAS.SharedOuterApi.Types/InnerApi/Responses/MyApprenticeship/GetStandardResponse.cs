using SFA.DAS.SharedOuterApi.Types.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.MyApprenticeship;

[ExcludeFromCodeCoverage]
public class GetStandardResponse
{
    public string? Title { get; set; }
    public int Level { get; set; }
    public string? Route { get; set; }
    public StandardVersionDetail? VersionDetail { get; set; }
}