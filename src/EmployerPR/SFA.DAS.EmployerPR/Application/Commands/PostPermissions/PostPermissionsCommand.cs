using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.EmployerPR.Application.Commands.PostPermissions;

public class PostPermissionsCommand : IRequest<Unit>
{
    public required Guid UserRef { get; set; }

    public long? Ukprn { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required List<Operation> Operations { get; set; } = [];
}