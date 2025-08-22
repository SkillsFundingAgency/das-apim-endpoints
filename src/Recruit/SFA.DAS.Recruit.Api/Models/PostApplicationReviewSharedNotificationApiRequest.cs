using System;

namespace SFA.DAS.Recruit.Api.Models;

public record PostApplicationReviewSharedNotificationApiRequest
{
    public required string HashAccountId { get; init; }
    public required long AccountId { get; init; }
    public required Guid VacancyId { get; init; }
    public required Guid ApplicationId { get; init; }
    public required string TrainingProvider { get; init; }
    public required string AdvertTitle { get; init; }
    public required long VacancyReference { get; init; }
}
