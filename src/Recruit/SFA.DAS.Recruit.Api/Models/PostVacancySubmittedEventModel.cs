using System;

namespace SFA.DAS.Recruit.Api.Models;

public class PostVacancySubmittedEventModel
{
    public Guid VacancyId { get; set; }
    public long VacancyReference { get; set; }
}