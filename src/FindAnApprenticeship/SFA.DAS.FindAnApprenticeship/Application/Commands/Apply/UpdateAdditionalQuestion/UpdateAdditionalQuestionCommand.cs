using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;
public class UpdateAdditionalQuestionCommand : IRequest<UpdateAdditionalQuestionQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public Guid Id { get; set; }
    public string Answer { get; set; }
    public int UpdatedAdditionalQuestion { get; set; }
    public SectionStatus AdditionalQuestionSectionStatus { get; set; }
}
