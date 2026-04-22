using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.MyApprenticeship
{
    [ExcludeFromCodeCoverage]
    public class GetStandardResponse
    {
        public string? Title { get; set; }
        public int Level { get; set; }
        public string? Route { get; set; }
        public StandardVersionDetail? VersionDetail { get; set; }
    }
}