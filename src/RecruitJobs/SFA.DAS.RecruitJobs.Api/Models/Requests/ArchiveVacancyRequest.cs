namespace SFA.DAS.RecruitJobs.Api.Models.Requests;

public sealed record ArchiveVacancyRequest
{
    public required Guid VacancyId { get; init; }
}