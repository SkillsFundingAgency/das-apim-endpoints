using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateApplicationQualification;

public class UpdateApplicationQualificationCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid QualificationReferenceId { get; set; }
    public List<Subject> Subjects { get; set; }

    public class Subject
    {
        public Guid Id { get; set; }
        public int? ToYear { get; set; }
        public string? Grade { get; set; }
        public string? Name { get; set; }
        public bool? IsPredicted { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}

