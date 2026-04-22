using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

public class TrainingProvider
{
    public long? Ukprn { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
}