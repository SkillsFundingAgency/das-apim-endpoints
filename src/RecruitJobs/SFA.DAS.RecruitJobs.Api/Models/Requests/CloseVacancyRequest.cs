namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public record CloseVacancyRequest
{
    public required Guid VacancyId { get; init; }
    public SharedOuterApi.Types.Domain.Recruit.ClosureReason ClosureReason { get; init; } = SharedOuterApi.Types.Domain.Recruit.ClosureReason.Auto;
}