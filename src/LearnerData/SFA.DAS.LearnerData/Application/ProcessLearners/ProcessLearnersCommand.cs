using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.ProcessLearners;

public class ProcessLearnersCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public DateTime ReceivedOn { get; set; }
    public int AcademicYear { get; set; }
    public IEnumerable<LearnerDataRequest> Learners { get; set; }
}

