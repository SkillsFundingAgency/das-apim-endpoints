using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;

public record ApplicationReviewSharedCommand(string HashAccountId,
    long AccountId,
    Guid VacancyId,
    Guid ApplicationId,
    string TrainingProvider,
    string AdvertTitle,
    long VacancyReference) : IRequest;