using System;

namespace SFA.DAS.Recruit.Api.Models;

public record PostSharedApplicationReviewedEventModel
{
    public required Guid VacancyId { get; set; }
    public required long VacancyReference { get; set; }
}