using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

namespace SFA.DAS.Apprenticeships.Api.Extensions;

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
}