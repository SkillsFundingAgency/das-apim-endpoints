using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class CreatePledgeRequest
    {
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsNamePublic { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DasAccountName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<string> Sectors { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<string> JobRoles { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<string> Levels { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<string> Locations { get; set; }

        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}