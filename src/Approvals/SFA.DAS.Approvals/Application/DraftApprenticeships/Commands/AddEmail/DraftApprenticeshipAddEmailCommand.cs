using MediatR;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;

public class DraftApprenticeshipAddEmailCommand : IRequest
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string Email { get; set; }
    public long ProviderId { get; set; }
}
