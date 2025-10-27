using MediatR;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Application.Commands
{
    public record ApprenticeshipStopCommand(long ProviderId, long LearnerDataId, ApprenticeshipStopRequest PatchRequest) : IRequest<bool>;

}

