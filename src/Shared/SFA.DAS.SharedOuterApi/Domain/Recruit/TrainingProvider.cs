using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit;

public class TrainingProvider
{
    public long? Ukprn { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
}