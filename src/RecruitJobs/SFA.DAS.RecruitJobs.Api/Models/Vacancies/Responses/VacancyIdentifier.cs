namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public record VacancyIdentifier(Guid Id,
    long? VacancyReference,
    Recruit.Contracts.ApiResponses.VacancyStatus Status,
    DateTime? ClosingDate);