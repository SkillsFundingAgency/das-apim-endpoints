using System;
using MediatR;

namespace SFA.DAS.Recruit.Events;

public record VacancySubmittedEvent(Guid VacancyId, long VacancyReference) : INotification;