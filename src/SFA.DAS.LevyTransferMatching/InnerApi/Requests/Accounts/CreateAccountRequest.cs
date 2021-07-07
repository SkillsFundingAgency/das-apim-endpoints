﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts
{
    public class CreateAccountRequest : IPostApiRequest
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }

        public string PostUrl => "/accounts";
        public object Data { get; set; }
    }
}
