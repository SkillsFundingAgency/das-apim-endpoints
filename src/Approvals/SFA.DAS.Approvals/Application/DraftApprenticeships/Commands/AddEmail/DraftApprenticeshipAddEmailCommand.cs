using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddEmail;

public class DraftApprenticeshipAddEmailCommand : IRequest<DraftApprenticeshipAddEmailResponse>
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string Email { get; set; }
    public long ProviderId { get; set; }
}
