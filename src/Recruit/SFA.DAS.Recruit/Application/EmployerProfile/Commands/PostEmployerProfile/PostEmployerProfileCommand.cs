#nullable enable
namespace SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;

public sealed record PostEmployerProfileCommand(long AccountId,
    long AccountLegalEntityId,
    string? AboutOrganisation,
    string? TradingName) : MediatR.IRequest;
