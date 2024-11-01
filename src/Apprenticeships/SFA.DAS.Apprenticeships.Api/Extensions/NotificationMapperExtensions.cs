using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
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
    }
}