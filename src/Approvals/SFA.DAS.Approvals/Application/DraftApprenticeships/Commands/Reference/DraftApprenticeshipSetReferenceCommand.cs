using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.Reference;

public class DraftApprenticeshipSetReferenceCommand: IRequest<DraftApprenticeshipSetReferenceResponse>
{
    public long CohortId { get; set; }
    public long DraftApprenticeshipId { get; set; }
    public string Reference { get; set; }
    public Party Party { get; set; }
}
