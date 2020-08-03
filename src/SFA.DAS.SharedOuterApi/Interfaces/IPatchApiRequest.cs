using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IPatchApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string PatchUrl { get; }
        object Data { get; set; }
    }
}