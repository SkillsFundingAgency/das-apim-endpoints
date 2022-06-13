using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class FindTraineeshipApiConfiguration : IInternalApiConfiguration
    {
    public string Url { get; set; }
    public string Identifier { get; set; }
    }
}
