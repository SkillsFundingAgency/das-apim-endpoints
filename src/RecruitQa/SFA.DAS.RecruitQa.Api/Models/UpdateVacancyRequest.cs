using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
using VacancyStatus = SFA.DAS.RecruitQa.Domain.VacancyStatus;

namespace SFA.DAS.RecruitQa.Api.Models;

public class UpdateVacancyRequest
{
    public required Guid Id { get; set; } 
    public required string Status { get; set; }
    public string OutcomeDescription { get; set; }
    public string TrainingDescription { get; set; }
    public string AdditionalTrainingDescription { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string WorkingWeekDescription { get; set; }
    public string CompanyBenefitsInformation { get; set; }
    public string EmployerLocationInformation { get; set; }
    public string ThingsToConsider { get; set; }
    public string ApplicationInstructions { get; set; }
}