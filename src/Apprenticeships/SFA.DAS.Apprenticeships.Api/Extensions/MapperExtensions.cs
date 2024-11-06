using SFA.DAS.Apprenticeships.Api.Models;
using PostCreateApprenticeshipPriceChangeRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships.PostCreateApprenticeshipPriceChangeRequest;

namespace SFA.DAS.Apprenticeships.Api.Extensions;

public static class MapperExtensions
{
    public static PostCreateApprenticeshipPriceChangeRequest ToApiRequest(this CreateApprenticeshipPriceChangeRequest request, Guid apprenticeshipKey)
    {
        return new PostCreateApprenticeshipPriceChangeRequest(
            apprenticeshipKey,
            request.Initiator,
            request.UserId,
            request.TrainingPrice,
            request.AssessmentPrice,
            request.TotalPrice,
            request.Reason,
            request.EffectiveFromDate);
    }
}