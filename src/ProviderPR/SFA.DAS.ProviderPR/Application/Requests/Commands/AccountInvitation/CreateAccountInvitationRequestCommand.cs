using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;

public class CreateAccountInvitationRequestCommand : IRequest<CreateAccountInvitationRequestCommandResult>
{
    public required long Ukprn { get; set; }
    public required string RequestedBy { get; set; }
    public required string EmployerOrganisationName { get; set; }
    public required string EmployerContactFirstName { get; set; }
    public required string EmployerContactLastName { get; set; }
    public required string EmployerContactEmail { get; set; }
    public required string EmployerPAYE { get; set; }
    public required string EmployerAORN { get; set; }
    public required Operation[] Operations { get; set; }
}
