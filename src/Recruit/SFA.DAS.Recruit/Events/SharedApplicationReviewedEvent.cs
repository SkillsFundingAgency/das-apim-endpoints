using MediatR;
using System;

namespace SFA.DAS.Recruit.Events;
public record SharedApplicationReviewedEvent(Guid VacancyId, long VacancyReference) : INotification;