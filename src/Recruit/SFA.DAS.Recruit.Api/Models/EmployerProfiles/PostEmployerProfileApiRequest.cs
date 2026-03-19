using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;

namespace SFA.DAS.Recruit.Api.Models.EmployerProfiles;

public sealed record PostEmployerProfileApiRequest
{
    public required long AccountId { get; init; }
    public string? AboutOrganisation { get; init; }
    public string? TradingName { get; init; }

    public PostEmployerProfileCommand ToCommand(long accountLegalEntityId)
    {
        return new PostEmployerProfileCommand(AccountId, accountLegalEntityId, AboutOrganisation, TradingName);
    }
}