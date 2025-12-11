using System;
using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Api.Models.Vacancies.Responses;

public class VacancyIdentity
{
    public Guid Id { get; set; }
    public long? VacancyReference { get; set; }
    public VacancyStatus Status { get; set; }
    public DateTime? ClosingDate { get; set; }
}