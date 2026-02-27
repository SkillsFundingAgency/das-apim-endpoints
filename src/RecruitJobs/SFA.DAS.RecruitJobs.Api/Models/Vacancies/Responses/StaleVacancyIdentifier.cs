using SFA.DAS.RecruitJobs.Domain.Vacancy;

namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public sealed record StaleVacancyIdentifier(Guid Id,
    long? VacancyReference,
    VacancyStatus Status,
    DateTime CreatedDate);
