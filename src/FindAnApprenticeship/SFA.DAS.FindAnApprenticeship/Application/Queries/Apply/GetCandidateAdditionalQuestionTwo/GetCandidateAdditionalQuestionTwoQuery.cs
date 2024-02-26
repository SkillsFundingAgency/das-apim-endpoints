using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;
public class GetCandidateAdditionalQuestionTwoQuery : IRequest<GetCandidateAdditionalQuestionTwoQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
