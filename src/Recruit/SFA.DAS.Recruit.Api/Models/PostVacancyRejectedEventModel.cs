using System;

namespace SFA.DAS.Recruit.Api.Models;

public class PostVacancyRejectedEventModel
{
    public required Guid VacancyId { get; set; }
}