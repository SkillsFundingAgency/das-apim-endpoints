using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateApprenticeshipForEditApiRequest(
    ValidateApprenticeshipForEditRequest validateApprenticeshipForEditRequest)
    : IPostApiRequest
{
    public ValidateApprenticeshipForEditRequest ValidateApprenticeshipForEditRequest { get; set; } = validateApprenticeshipForEditRequest;
    public string PostUrl => "api/apprenticeships/edit/validate";
    public object Data { get; set; } = validateApprenticeshipForEditRequest;
}