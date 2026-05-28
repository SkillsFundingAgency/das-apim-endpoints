using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public record VacancyIdentifier(Guid Id,
    long? VacancyReference,
    VacancyStatus Status,
    DateTime? ClosingDate);