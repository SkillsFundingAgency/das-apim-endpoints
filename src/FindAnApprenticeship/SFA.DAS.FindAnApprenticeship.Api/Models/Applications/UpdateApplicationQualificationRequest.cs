using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class UpdateApplicationQualificationRequest
{
    public Guid CandidateId { get; set; }
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
