using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

public class DraftApprenticeshipSetReferenceCommand: IRequest
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string Reference { get; set; }
    public long ProviderId { get; set; }
}
