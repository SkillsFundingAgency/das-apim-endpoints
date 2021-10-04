using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Responses
{
    public class GetQualificationsResponse
    {
        [JsonProperty("Qualifications")]
        public List<GetQualificationsItem> Qualifications { get; set; }
    }

    public class GetQualificationsItem
    {
        [JsonProperty("GcseOrEquivalent")]
        public string GcseOrEquivalent { get; set; }

        [JsonProperty("AsLevelOrEquivalent")]
        public string AsLevelOrEqivalant { get; set; }

        [JsonProperty("ALevelOrEquivalent")]
        public string ALevelOrEquivalent { get; set; }

        [JsonProperty("BtecOrEquivalent")]
        public string BtecOrEquivalent { get; set; }

        [JsonProperty("NvqOrSvqLevel1OrEquivalent")]
        public string NvqOrSvqLevel1OrEquivalent { get; set; }

        [JsonProperty("NvqOrSvqLevel2OrEquivalent")]
        public string NvqOrSvqLevel2OrEquivalent { get; set; }

        [JsonProperty("NvqOrSvqLevel3OrEquivalent")]
        public long NvqOrSvqLevel3OrEquivalent { get; set; }

        [JsonProperty("Other")]
        public long Other { get; set; }
    }
}