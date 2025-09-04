using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Events.ApplicationReviewShared;

public record ApplicationReviewSharedEvent(string HashAccountId,
    long AccountId,
    Guid VacancyId,
    Guid ApplicationId,
    string TrainingProvider,
    string AdvertTitle,
    long VacancyReference) : INotification;