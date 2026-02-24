using SFA.DAS.RecruitJobs.Domain.Vacancy;

namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public record VacancyIdentifier(
    Guid Id,
    long? VacancyReference,
    VacancyStatus Status,
    DateTime? ClosingDate);