using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests
{
    public class ProviderCourseSortOrder
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SortOrder : short
        {
            Distance = 0,
            Name = 1
        }
    }
}