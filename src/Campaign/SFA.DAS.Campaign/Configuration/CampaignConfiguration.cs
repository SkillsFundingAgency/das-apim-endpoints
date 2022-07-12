
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models;


namespace SFA.DAS.Campaign.Configuration
{
    public class CampaignConfiguration
    {
        public string ApimEndpointsRedisConnectionString { get ; set ; }
        public string FindAnApprenticeshipBaseUrl { get; set; }
        public IEnumerable<OnsProductivityDataItem> OnsProductivity { get; set; }
    }
}