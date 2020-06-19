using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Interfaces
{
    public interface IGetAllApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetAllUrl { get; }
    }
}