using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationFormsApiRequest : IGetApiRequest
{
    public string GetUrl => $"/api/applications/forms";
}
