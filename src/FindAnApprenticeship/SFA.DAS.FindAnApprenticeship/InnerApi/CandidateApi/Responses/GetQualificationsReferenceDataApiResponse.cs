using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetQualificationsReferenceDataApiResponse
    {
        public List<QualificationType> QualificationReferences { get; set; }
    }

    public class QualificationType
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public short Order { get; set; }
    }
}
