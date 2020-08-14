﻿using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class GetAccountLegalEntityRequest : IGetApiRequest
    {
        private readonly string _accountId;
        private readonly long _legalEntityId;

        public GetAccountLegalEntityRequest(string accountId, long legalEntityId)
        {
            _accountId = accountId;
            _legalEntityId = legalEntityId;
        }

        public string BaseUrl { get; set; }

        public string GetUrl =>
            $"{BaseUrl}api/accounts/{_accountId}/legalentities/{_legalEntityId}";
    }
}