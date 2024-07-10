using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateInterviewAdjustments;
public class UpsertInterviewAdjustmentsCommand : IRequest<UpsertInterviewAdjustmentsCommandResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public string InterviewAdjustmentsDescription { get; set; }
    public SectionStatus InterviewAdjustmentsSectionStatus { get; set; }
}
