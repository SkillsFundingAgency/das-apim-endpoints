using MediatR;
using System;

namespace SFA.DAS.Recruit.Events;

public record ApplicationSubmittedEvent(Guid ApplicationId, Guid VacancyId) : INotification;
