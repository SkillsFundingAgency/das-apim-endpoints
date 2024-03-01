using MediatR;
using SFA.DAS.FindAnApprenticeship.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationAdditionalQuestion;

public record PatchApplicationAdditionalQuestionCommand : IRequest<PatchApplicationAdditionalQuestionCommandResponse>
{
    public Guid ApplicationId { set; get; }
    public Guid CandidateId { set; get; }
    public SectionStatus? AdditionalQuestionOne { get; set; }
    public SectionStatus? AdditionalQuestionTwo { get; set; }
}