using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationsToAutoDeclineRequest() : IGetApiRequest
{
    public string GetUrl => "/applications-auto-decline";
}