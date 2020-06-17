using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces
{
    public interface IGetAllApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string GetAllUrl { get; }
    }
}