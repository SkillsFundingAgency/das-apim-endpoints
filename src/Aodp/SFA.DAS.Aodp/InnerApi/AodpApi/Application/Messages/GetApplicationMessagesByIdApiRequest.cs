using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

public class GetApplicationMessagesByIdApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }
    public string GetUrl => $"/api/applications/{ApplicationId}/messages";
}
