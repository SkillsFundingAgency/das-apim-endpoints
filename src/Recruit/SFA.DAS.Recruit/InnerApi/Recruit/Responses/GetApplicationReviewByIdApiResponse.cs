using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses;
public record GetApplicationReviewByIdApiResponse
{
    public ApplicationReview ApplicationReview { get; set; }
}