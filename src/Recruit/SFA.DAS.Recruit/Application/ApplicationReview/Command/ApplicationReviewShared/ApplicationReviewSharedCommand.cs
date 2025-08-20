using System;
using MediatR;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.ApplicationReviewShared;

public record ApplicationReviewSharedCommand(string HashAccountId,
    Guid VacancyId,
    Guid ApplicationId,
    string RecipientEmail,
    string FirstName,
    string TrainingProvider,
    string AdvertTitle,
    long VacancyReference) : IRequest;