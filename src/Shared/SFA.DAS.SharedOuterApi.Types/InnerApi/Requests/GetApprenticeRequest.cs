using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetApprenticeRequest(Guid id) : IGetApiRequest
{
    public Guid Id { get; } = id;

    public string GetUrl => $"apprentices/{Id}";
}