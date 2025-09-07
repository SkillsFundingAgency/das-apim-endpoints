using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.SyncLearnerData;

public class SyncLearnerDataCommand : IRequest<SyncLearnerDataCommandResult>
{
    public long ProviderId { get; set; }
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
}

    public class SyncLearnerDataCommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public GetDraftApprenticeshipResponse UpdatedDraftApprenticeship { get; set; }
    }
