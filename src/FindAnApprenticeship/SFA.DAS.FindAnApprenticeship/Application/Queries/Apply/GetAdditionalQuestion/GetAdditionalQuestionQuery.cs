using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
public class GetAdditionalQuestionQuery : IRequest<GetAdditionalQuestionQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid Id { get; set; }
    public int AdditionalQuestion { get; set; }
}
