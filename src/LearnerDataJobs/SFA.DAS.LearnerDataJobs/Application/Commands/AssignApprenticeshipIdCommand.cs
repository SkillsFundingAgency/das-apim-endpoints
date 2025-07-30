using MediatR;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Application.Commands;

public class AssignApprenticeshipIdCommand : IRequest<bool>
{
    public long ProviderId { get; set; }
    public long LearnerDataId { get; set; }
    public LearnerDataApprenticeshipIdRequest PatchRequest { get; set; }
}
