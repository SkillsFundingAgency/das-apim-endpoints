using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

public class CreateApplicationMessageApiRequest : IPostApiRequest
{
    public Guid ApplicationId { get; set; }
    public string PostUrl => $"/api/applications/{ApplicationId}";
    public object Data { get; set; }
}
