namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public sealed record StaleArchiveVacancyIdentifier(Guid Id, 
    long? VacancyReference, 
    Recruit.Contracts.ApiResponses.VacancyStatus Status, 
    DateTime? ClosingDate) : VacancyIdentifier(Id, VacancyReference, Status, ClosingDate);