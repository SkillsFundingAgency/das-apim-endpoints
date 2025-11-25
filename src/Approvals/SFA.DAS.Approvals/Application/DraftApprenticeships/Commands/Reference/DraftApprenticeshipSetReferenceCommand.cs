using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

public class DraftApprenticeshipSetReferenceCommand: IRequest<Unit>
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string Reference { get; set; }
    public Party Party { get; set; }
}
