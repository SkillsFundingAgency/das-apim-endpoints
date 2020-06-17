using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces
{
    public interface IBaseApiRequest
    {
        [JsonIgnore]
        string BaseUrl { get; set; }
    }
}
