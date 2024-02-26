using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmployerAdditionalQuestionTwo;
public class GetEmployerAdditionalQuestionTwoQuery : IRequest<GetEmployerAdditionalQuestionTwoQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
