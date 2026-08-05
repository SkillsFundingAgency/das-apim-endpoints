namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public sealed record StaleVacancyIdentifier(Guid Id,
    long? VacancyReference,
    Recruit.Contracts.ApiResponses.VacancyStatus Status,
    DateTime CreatedDate);
