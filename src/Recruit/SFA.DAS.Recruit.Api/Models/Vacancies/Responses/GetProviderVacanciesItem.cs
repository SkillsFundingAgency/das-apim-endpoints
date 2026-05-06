using System;
using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Api.Models.Vacancies.Responses;

public class GetProviderVacanciesItem
{
    public Guid Id { get; set; }
    public OwnerType OwnerType { get; set; }
    public VacancyStatus Status { get; set; }
    public long? AccountId { get; set; }
    public long? VacancyReference { get; set; }
}