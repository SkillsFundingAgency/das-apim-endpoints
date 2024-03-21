using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQueryResult
{
    public string QualificationReference { get; set; }
    public List<Qualification> Qualifications { get; set; }

    public class Qualification
    {
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public string? AdditionalInformation { get; set; }
        public bool? IsPredicted { get; set; }
    }
}