using SFA.DAS.Recruit.Domain;
namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetApplicationReviewByIdApiResponse
{
    public ApplicationReview ApplicationReview { get; set; }
}