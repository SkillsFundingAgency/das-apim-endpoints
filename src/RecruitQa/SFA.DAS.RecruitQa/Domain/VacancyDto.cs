using SFA.DAS.SharedOuterApi.Types.Domain;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.RecruitQa.Domain;

public class VacancyDto
{
    public long? VacancyReference { get; init; }
    public string? Title { get; init; }
    
    public DateTime? StartDate { get; init; }
    public List<Address>? EmployerLocations { get; set; }
    public SharedOuterApi.Types.Domain.AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerName { get; init; }
    public int? NumberOfPositions { get; init; }
    public string? ProgrammeId { get; set; }
    public Wage? Wage { get; set; }
    public required VacancyStatus Status { get; init; }
}