using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public class UpdateApplicationModel
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public SectionStatus WorkHistorySectionStatus { get; set; }
    }
}