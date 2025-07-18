using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public record PostEmploymentLocationsApiRequest
    {
        public Guid CandidateId { get; set; }
        public InnerApi.CandidateApi.Shared.LocationDto EmployerLocation { get; set; }
        public SectionStatus EmploymentLocationSectionStatus { get; set; } = SectionStatus.NotStarted;
    }
}