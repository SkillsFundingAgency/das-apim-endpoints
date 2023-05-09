using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;

public class GetApprenticeRequest : IGetApiRequest
{
    public Guid ApprenticeId { get; }
    public string GetUrl => $"apprentices/{ApprenticeId}";

    public GetApprenticeRequest(Guid apprenticeId) => ApprenticeId = apprenticeId;
}
