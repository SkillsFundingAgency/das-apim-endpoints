using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQuery : IRequest<GetInterviewAdjustmentsQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
