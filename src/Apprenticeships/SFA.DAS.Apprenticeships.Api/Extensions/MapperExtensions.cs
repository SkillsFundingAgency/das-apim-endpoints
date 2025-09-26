using SFA.DAS.Apprenticeships.Api.Models;
using PostCreateApprenticeshipPriceChangeRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning.PostCreateApprenticeshipPriceChangeRequest;
using PatchApproveApprenticeshipPriceChangeRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning.PatchApproveApprenticeshipPriceChangeRequest;
using PatchRejectApprenticeshipPriceChangeRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning.PatchRejectApprenticeshipPriceChangeRequest;

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

    public static PatchApproveApprenticeshipPriceChangeRequest ToApiRequest(this ApprovePriceChangeRequest request, Guid apprenticeshipKey)
    {
        return new PatchApproveApprenticeshipPriceChangeRequest(
            apprenticeshipKey,
            request.UserId,
            request.TrainingPrice,
            request.AssessmentPrice);
    }

    public static PatchRejectApprenticeshipPriceChangeRequest ToApiRequest(this RejectPriceChangeRequest request, Guid apprenticeshipKey)
    {
        return new PatchRejectApprenticeshipPriceChangeRequest(
            apprenticeshipKey,
            request.Reason);
    }

}