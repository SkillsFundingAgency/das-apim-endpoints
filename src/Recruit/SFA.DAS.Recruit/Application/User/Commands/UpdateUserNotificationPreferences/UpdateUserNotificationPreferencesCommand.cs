using System;
using MediatR;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Application.User.Commands.UpdateUserNotificationPreferences;

public record UpdateUserNotificationPreferencesCommand(Guid Id, NotificationPreferences NotificationPreferences): IRequest<bool>;