using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetMultipleEligibleApprenticeshipsRequest : IPostApiRequest
    {
        private long _accountId;
        private long _accountLegalEntityId;

        public GetMultipleEligibleApprenticeshipsRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string BaseUrl { get; set; }

        public string PostUrl => $"{BaseUrl}eligible-apprenticeships/{_accountId}/{_accountLegalEntityId}";

        public object Data { get; set; }
    }

    public class EligibleApprenticeshipResult
    {
        public long Uln { get; set; }
        public bool Eligible { get; set; }
    }
}