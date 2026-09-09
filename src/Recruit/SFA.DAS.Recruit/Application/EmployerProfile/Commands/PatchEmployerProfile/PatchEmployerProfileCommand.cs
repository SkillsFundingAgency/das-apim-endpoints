namespace SFA.DAS.Recruit.Application.EmployerProfile.Commands.PatchEmployerProfile;

public record PatchEmployerProfileCommand(
    long AccountLegalEntityId,
    InnerApi.Models.EmployerProfile EmployerProfile) : MediatR.IRequest;