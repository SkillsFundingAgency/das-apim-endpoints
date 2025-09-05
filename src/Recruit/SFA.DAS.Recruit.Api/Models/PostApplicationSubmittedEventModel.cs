using System;

namespace SFA.DAS.Recruit.Api.Models;

public class PostApplicationSubmittedEventModel
{
    public Guid ApplicationId { get; set; }
    public long VacancyReference { get; set; }
}
