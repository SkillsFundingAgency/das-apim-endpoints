using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IPostApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string PostUrl { get; }
        object Data { get; set; }
    }
}