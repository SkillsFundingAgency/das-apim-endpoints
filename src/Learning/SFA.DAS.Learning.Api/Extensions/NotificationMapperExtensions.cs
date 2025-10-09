using SFA.DAS.Learning.Api.Models;
using SFA.DAS.Learning.Application.Notification.Handlers;

namespace SFA.DAS.Learning.Api.Extensions;

public static class NotificationMapperExtensions
{
    public static ApprenticeshipWithdrawnCommand ToNotificationCommand(this HandleWithdrawalNotificationsRequest request, Guid apprenticeshipKey)
    {
        return new ApprenticeshipWithdrawnCommand
        {
            ApprenticeshipKey = apprenticeshipKey,
            LastDayOfLearning = request.LastDayOfLearning,
            Reason = request.Reason
        };
    }
}
