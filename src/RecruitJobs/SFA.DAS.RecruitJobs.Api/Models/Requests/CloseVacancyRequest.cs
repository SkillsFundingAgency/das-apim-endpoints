using SFA.DAS.RecruitJobs.Domain.Vacancy;

namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public record CloseVacancyRequest
{
    public required Guid VacancyId { get; init; }
    public ClosureReason ClosureReason { get; init; } = ClosureReason.Auto;
}