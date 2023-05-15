using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;

public record PostMyApprenticeshipRequest(Guid Id) : IPostApiRequest
{
    public object Data { get; set; } = null!;
    public string PostUrl => $"apprentices/{Id}/MyApprenticeship";
}
