using MediatR;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Application.Commands;
public record ApprenticeshipStopDateChangedCommand(long ProviderId, long LearnerDataId, ApprenticeshipStopRequest PatchRequest) : IRequest<bool>;

