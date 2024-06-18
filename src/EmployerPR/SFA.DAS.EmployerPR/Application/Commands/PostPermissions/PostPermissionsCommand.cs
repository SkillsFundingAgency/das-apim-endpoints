using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

namespace SFA.DAS.EmployerPR.Application.Commands.PostPermissions;

public class PostPermissionsCommand : IRequest<PostPermissionsCommandResult>
{
    public required Guid UserRef { get; set; }

    public long? Ukprn { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required List<Operation> Operations { get; set; } = [];
}