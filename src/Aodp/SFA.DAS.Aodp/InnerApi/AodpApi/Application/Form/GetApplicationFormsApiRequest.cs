using SFA.DAS.SharedOuterApi.Configuration;using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationFormsApiRequest : IGetApiRequest
{
    public string GetUrl => $"/api/applications/forms";
}
