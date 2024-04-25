using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications
{
    public class PostDeleteQualificationsApiRequest
    {
        public Guid CandidateId { get; set; }
        public Guid? Id { get; set; }
    }
}
