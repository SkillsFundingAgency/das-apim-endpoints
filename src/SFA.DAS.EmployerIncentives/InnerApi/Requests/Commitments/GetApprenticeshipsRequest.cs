using System;
using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments
{
    public class GetApprenticeshipsRequest : IGetApiRequest
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;
        private readonly DateTime _startDateFrom;
        private readonly DateTime _startDateTo;

        public GetApprenticeshipsRequest(long accountId, long accountLegalEntityId, DateTime startDateFrom, DateTime startDateTo)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
            _startDateFrom = startDateFrom;
            _startDateTo = startDateTo;
        }

        public string GetUrl =>
            $"api/apprenticeships?accountId={_accountId}&accountLegalEntityId={_accountLegalEntityId}&startDateRangeFrom={WebUtility.UrlEncode(_startDateFrom.ToString("u"))}&startDateRangeTo={WebUtility.UrlEncode(_startDateTo.ToString("u"))}";
    }
}