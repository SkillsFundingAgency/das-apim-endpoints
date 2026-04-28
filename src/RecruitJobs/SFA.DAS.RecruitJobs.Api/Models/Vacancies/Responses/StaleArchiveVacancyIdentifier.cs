using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.Api.Models.Vacancies.Responses;

public sealed record StaleArchiveVacancyIdentifier(Guid Id, 
    long? VacancyReference, 
    VacancyStatus Status, 
    DateTime? ClosingDate) : VacancyIdentifier(Id, VacancyReference, Status, ClosingDate);
