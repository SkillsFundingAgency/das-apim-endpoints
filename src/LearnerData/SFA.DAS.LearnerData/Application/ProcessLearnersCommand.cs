using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application;

public class ProcessLearnersCommand : IRequest
{
    public IEnumerable<LearnerDataRequest> Learners { get; set; }
}

