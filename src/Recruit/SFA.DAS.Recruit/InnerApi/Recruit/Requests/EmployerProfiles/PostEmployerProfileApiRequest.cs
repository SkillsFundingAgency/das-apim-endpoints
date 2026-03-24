#nullable enable
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles.PostEmployerProfileApiRequest;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests.EmployerProfiles;

public sealed record PostEmployerProfileApiRequest(long AccountLegalEntityId, PostEmployerProfileApiRequestData Payload) : IPutApiRequest
{
    public string PutUrl => $"api/employer/profiles/{AccountLegalEntityId}";
    public object Data { get; set; } = Payload;

    public record PostEmployerProfileApiRequestData
    {
        public required long AccountId { get; init; }
        public string? TradingName { get; init; } = null;
        public string? AboutOrganisation { get; init; } = null;
    }
}