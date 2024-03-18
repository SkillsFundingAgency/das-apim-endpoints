using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class QualificationReferenceDataApiResponseItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public short Order { get; set; }
    }
}
