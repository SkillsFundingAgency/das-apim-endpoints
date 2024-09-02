using MediatR;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;

public class AddAccountRequestCommand : IRequest<AddAccountRequestCommandResult>
{
    public required long AccountId { get; set; }

    public required long? Ukprn { get; set; }

    public required string RequestedBy { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public string? EmployerContactEmail { get; set; }

    public required List<Operation> Operations { get; set; } = [];
}
