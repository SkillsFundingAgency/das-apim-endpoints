using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IBaseApiRequest 
    {
        [JsonIgnore]
        string BaseUrl { get; set; }
        [JsonIgnore]
        string Version => "1.0";
    }
}
