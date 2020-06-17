using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces
{
    public interface IGetApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}
