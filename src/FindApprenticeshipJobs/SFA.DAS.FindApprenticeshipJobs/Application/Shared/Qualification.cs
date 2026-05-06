using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared;
public class Qualification
{
    public string QualificationType { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Grade { get; set; } = null!;
    public QualificationWeighting? Weighting { get; set; }
}
