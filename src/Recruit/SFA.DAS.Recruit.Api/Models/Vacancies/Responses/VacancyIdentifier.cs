using System;
using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Api.Models.Vacancies.Responses;

public record VacancyIdentifier(
    Guid Id,
    long? VacancyReference,
    VacancyStatus Status,
    DateTime? ClosingDate);