using SFA.DAS.RecruitJobs.Domain;

namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public record VacancyIdentifier(
    Guid Id,
    long? VacancyReference,
    VacancyStatus Status,
    DateTime? ClosingDate);