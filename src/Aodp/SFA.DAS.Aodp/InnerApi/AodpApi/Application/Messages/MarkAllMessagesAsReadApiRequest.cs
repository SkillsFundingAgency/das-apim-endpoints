using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

public class MarkAllMessagesAsReadApiRequest : IPutApiRequest
{
    public Guid ApplicationId { get; set; }
    public string PutUrl => $"/api/applications/{ApplicationId}/messages/read";
    public object Data { get; set; }
}
