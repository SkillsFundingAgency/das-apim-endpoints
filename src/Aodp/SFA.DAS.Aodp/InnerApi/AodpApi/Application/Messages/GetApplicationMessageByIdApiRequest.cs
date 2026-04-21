using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;

public class GetApplicationMessageByIdApiRequest : IGetApiRequest
{
    public Guid MessageId { get; set; }
    public string GetUrl => $"api/applications/messages/{MessageId}";
}