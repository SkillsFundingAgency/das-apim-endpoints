using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetApprenticeRequest : IGetApiRequest
{
    public Guid Id { get; }

    public GetApprenticeRequest(Guid id)
    {
        Id = id;
    }

    public string GetUrl => $"apprentices/{Id}";
}