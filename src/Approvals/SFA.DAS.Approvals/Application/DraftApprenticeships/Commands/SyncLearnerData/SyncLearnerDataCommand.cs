using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;

public class SyncLearnerDataCommand : IRequest<GetDraftApprenticeshipResponse>
{
    public long ProviderId { get; set; }
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
}

