using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class ApplicationRejectRequest
    {
        public List<string> ApplicationsToReject { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}