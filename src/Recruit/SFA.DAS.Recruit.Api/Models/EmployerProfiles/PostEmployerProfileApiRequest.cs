#nullable enable
using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;

namespace SFA.DAS.Recruit.Api.Models.EmployerProfiles;

public sealed record PostEmployerProfileApiRequest
{
    public required long AccountId { get; init; }
    public string? AboutOrganisation { get; init; } = null;
    public string? TradingName { get; init; } = null;

    public PostEmployerProfileCommand ToCommand(long accountLegalEntityId)
    {
        return new PostEmployerProfileCommand(AccountId, accountLegalEntityId, AboutOrganisation, TradingName);
    }
}