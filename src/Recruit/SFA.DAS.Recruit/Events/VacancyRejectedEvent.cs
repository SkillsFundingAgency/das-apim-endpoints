using System;
using MediatR;

namespace SFA.DAS.Recruit.Events;

public record VacancyRejectedEvent(Guid Id): INotification;