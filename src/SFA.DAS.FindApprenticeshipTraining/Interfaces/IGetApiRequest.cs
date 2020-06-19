using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Interfaces
{
    public interface IGetApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}
