using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

public class GetMyApprenticeshipRequest : IGetApiRequest
{
    public Guid Id { get; }

    public GetMyApprenticeshipRequest(Guid id)
    {
        Id = id;
    }

    public string GetUrl => $"apprentices/{Id}/MyApprenticeship";
}