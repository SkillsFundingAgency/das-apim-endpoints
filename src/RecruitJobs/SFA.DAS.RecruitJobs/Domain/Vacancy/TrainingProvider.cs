using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

public class TrainingProvider
{
    public long? Ukprn { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
}