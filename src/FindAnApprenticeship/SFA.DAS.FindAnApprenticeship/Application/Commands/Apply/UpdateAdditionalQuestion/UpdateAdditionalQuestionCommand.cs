using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
public class UpdateAdditionalQuestionCommand : IRequest<UpdateAdditionalQuestionQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}
