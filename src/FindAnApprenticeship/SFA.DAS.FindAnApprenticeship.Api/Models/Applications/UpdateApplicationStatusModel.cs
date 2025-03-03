using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications
{
    public record UpdateApplicationStatusModel
    {
        public ApplicationStatus Status { get; set; }
    }
}