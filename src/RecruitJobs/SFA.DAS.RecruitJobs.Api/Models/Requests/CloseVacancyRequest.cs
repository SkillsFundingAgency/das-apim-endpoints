namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public record CloseVacancyRequest
{
    public required Guid VacancyId { get; init; }
    public Recruit.Contracts.ApiResponses.ClosureReason ClosureReason { get; init; } = Recruit.Contracts.ApiResponses.ClosureReason.Auto;
}