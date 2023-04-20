using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

public class GetStandardResponse{
    public string Title { get; set; }
    public int Level { get; set; }
    public string Route { get; set; }
    public StandardVersionDetail VersionDetail { get; set; }
}