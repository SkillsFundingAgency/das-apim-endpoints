using MediatR;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.CreateLearner;

#pragma warning disable CS8618
public class CreateLearnerCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public DateTime ReceivedOn { get; set; }
    public CreateLearnerRequest Request { get; set; }
    public long Ukprn { get; set; }
}
#pragma warning restore CS8618
