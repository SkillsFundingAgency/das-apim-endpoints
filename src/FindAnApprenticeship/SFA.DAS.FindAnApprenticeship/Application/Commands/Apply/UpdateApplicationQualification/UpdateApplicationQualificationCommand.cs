using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;

public class UpdateApplicationQualificationCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid Id { get; set; }
    public int? ToYear { get; set; }
    public string? Grade { get; set; }
    public string? Subject { get; set; }
    public bool? IsPredicted { get; set; }
    public string? AdditionalInformation { get; set; }
    public Guid QualificationReferenceId { get; set; }
}