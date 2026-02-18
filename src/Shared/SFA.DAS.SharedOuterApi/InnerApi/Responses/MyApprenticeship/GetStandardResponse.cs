using SFA.DAS.SharedOuterApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship
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