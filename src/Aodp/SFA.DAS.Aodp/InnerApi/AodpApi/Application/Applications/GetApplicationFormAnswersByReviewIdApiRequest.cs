using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Applications;

public class GetApplicationFormAnswersByReviewIdApiRequest : IGetApiRequest
{
    public Guid ApplicationReviewId { get; set; }
    public string GetUrl => $"/api/application-reviews/{ApplicationReviewId}/form-answers";
}
