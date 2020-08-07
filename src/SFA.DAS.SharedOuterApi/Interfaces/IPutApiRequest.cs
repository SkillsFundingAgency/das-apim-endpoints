using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IPutApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string PutUrl { get; }
        object Data { get; set; }
    }

    public interface IPutApiRequest<TData> : IBaseApiRequest
    {
        [JsonIgnore]
        string PutUrl { get; }
        TData Data { get; set; }
    }
}