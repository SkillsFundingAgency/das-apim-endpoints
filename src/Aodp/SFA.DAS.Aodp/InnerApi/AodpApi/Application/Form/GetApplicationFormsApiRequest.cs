using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormsApiRequest : IGetApiRequest
{
    public string GetUrl => $"/api/applications/forms";
}
