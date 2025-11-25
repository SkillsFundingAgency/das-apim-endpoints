using MediatR;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries;

public class ValidateEmailOverlapQuery : IRequest<ValidateEmailOverlapQueryResult>
{
    public long DraftApprenticeshipId { get; set; }
    public string Email { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public long CohortId { get; set; }
}
