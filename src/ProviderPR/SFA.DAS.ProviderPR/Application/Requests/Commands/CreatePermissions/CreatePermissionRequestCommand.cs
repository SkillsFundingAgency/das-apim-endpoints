using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;

public class CreatePermissionRequestCommand : IRequest<CreatePermissionRequestCommandResult>
{
    public required long? Ukprn { get; set; }

    public required string RequestedBy { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required List<Operation> Operations { get; set; } = [];

    public required long AccountId { get; set; }
}
