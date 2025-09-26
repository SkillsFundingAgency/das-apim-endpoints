using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using CreateApprenticeshipPriceChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipPriceChangeRequest;
using CreateApprenticeshipStartDateChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipStartDateChangeRequest;

namespace SFA.DAS.Apprenticeships.Api.Extensions
{
    public static class NotificationMapperExtensions
    {
        public static ChangeOfPriceInitiatedCommand ToNotificationCommand(this CreateApprenticeshipPriceChangeRequest request, Guid apprenticeshipKey, PostCreateApprenticeshipPriceChangeApiResponse response)
        {
            return new ChangeOfPriceInitiatedCommand
            {
                Initiator = request.Initiator,
                ApprenticeshipKey = apprenticeshipKey,
                PriceChangeStatus = response.PriceChangeStatus
            };
        }

        public static ChangeOfStartDateInitiatedCommand ToNotificationCommand(this CreateApprenticeshipStartDateChangeRequest request, Guid apprenticeshipKey)
        {
            return new ChangeOfStartDateInitiatedCommand
            {
                Initiator = request.Initiator,
                ApprenticeshipKey = apprenticeshipKey
            };
        }

        public static ChangeOfPriceApprovedCommand ToNotificationCommand(this PatchApproveApprenticeshipPriceChangeResponse response, Guid apprenticeshipKey)
        {
            return new ChangeOfPriceApprovedCommand
            {
                ApprenticeshipKey = apprenticeshipKey,
                Approver = response.Approver
            };
        }
        public static ChangeOfDateApprovedCommand ToNotificationCommand(this ApproveStartDateChangeRequest request, Guid apprenticeshipKey)
        {
            return new ChangeOfDateApprovedCommand
            {
                ApprenticeshipKey = apprenticeshipKey,                
            };
        }

        public static ChangeOfPriceRejectedCommand ToNotificationCommand(this PatchRejectApprenticeshipPriceChangeResponse response, Guid apprenticeshipKey)
        {
            return new ChangeOfPriceRejectedCommand
            {
                ApprenticeshipKey = apprenticeshipKey,
                Rejector = response.Rejector
            };
        }

        public static PaymentStatusInactiveCommand ToNotificationCommand(this FreezePaymentsRequest request, Guid apprenticeshipKey)
        {
            return new PaymentStatusInactiveCommand
            {
                ApprenticeshipKey = apprenticeshipKey
            };
        }

        public static PaymentStatusActiveCommand ToNotificationCommand(this PostUnfreezePaymentsRequest request, Guid apprenticeshipKey)
        {
            return new PaymentStatusActiveCommand
            {
                ApprenticeshipKey = apprenticeshipKey
            };
        }
        
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
}