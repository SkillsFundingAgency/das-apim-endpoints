using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestionTwo;
public class GetAdditionalQuestionTwoQuery : IRequest<GetAdditionalQuestionTwoQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
