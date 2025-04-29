using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

public class GetApplicationMessagesByApplicationIdApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }
    public string UserType { get; set; }
    public string GetUrl => $"/api/applications/{ApplicationId}/messages?userType={UserType}";
}
