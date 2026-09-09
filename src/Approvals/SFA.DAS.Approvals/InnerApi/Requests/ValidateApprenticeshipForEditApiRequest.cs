using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateApprenticeshipForEditApiRequest(
    ValidateApprenticeshipForEditRequest validateApprenticeshipForEditRequest)
    : IPostApiRequest
{
    public ValidateApprenticeshipForEditRequest ValidateApprenticeshipForEditRequest { get; set; } = validateApprenticeshipForEditRequest;
    public string PostUrl => "api/apprenticeships/edit/validate";
    public object Data { get; set; } = validateApprenticeshipForEditRequest;
}