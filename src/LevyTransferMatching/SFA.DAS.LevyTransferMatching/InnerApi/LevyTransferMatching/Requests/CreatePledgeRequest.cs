using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class CreatePledgeRequest : IPostApiRequest
    {
        private readonly long _accountId;

        public CreatePledgeRequest(long accountId, CreatePledgeRequestData data)
        {
            _accountId = accountId;
            Data = data;
        }

        public string PostUrl => $"accounts/{_accountId}/pledges";

        public object Data { get; set; }

        public class CreatePledgeRequestData
        {
            public long AccountId { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public bool IsNamePublic { get; set; }
            public string DasAccountName { get; set; }
            public DateTime CreatedOn { get; set; }
            public IEnumerable<string> Sectors { get; set; }
            public IEnumerable<string> JobRoles { get; set; }
            public IEnumerable<string> Levels { get; set; }
            public List<LocationDataItem> Locations { get; set; }
            public string UserId { get; set; }
            public string UserDisplayName { get; set; }
        }
    }
}
