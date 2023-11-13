using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Responses;

public class GetStandardResponse
{
    public string? Title { get; set; }
    public int Level { get; set; }
    public string? Route { get; set; }
    public StandardVersionDetail? VersionDetail { get; set; }
}