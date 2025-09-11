using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.ClearLearnerDataFlags;

public class ClearLearnerDataFlagsCommand : IRequest<Unit>
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public long ProviderId { get; set; }
}
