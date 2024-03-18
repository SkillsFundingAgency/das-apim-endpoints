using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

public class GetQualificationsQueryResult
{
    public bool? IsSectionCompleted { get; set; }
    public List<Qualification> Qualifications { get; set; }
    public List<QualificationReferenceDataItem> QualificationTypes { get; set; }

    public class Qualification
    {
        public Guid Id { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public string? AdditionalInformation { get; set; }
        public bool? IsPredicted { get; set; }
    }

    public class QualificationReferenceDataItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public short Order { get; set; }
    }
}