﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostBankDetailsRequiredEmailRequest : IPostApiRequest
    {
        public PostBankDetailsRequiredEmailRequest(long accountId)
        {
            AccountId = accountId;
        }

        public long AccountId { get; private set; }
        public string PostUrl => $"api/EmailCommand/bank-details-required";

        public object Data { get; set; }
    }
}
