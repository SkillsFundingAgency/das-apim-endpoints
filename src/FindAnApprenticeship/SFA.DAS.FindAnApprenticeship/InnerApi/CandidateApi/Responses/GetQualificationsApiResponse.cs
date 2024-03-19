using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetQualificationsApiResponse
    {
        public List<Qualification> Qualifications { get; set; } = null!;

        public class Qualification
        {
            public Guid Id { get; set; }
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
            public QualificationReference QualificationReference { get; set; }
        }

        public class QualificationReference
        {
            public Guid Id { get; set; }
        }
    }
}

