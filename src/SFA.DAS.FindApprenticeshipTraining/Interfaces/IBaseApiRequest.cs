using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Interfaces
{
    public interface IBaseApiRequest
    {
        [JsonIgnore]
        string BaseUrl { get; set; }
    }
}
