using System;

namespace SFA.DAS.Recruit.Api.Models;

public class PostApplicationSubmittedEventModel
{
    public Guid ApplicationId { get; set; }
    public Guid VacancyId { get; set; }
}
