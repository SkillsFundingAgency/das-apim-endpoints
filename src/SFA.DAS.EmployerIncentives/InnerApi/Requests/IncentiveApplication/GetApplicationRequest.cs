using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class GetApplicationRequest : IGetApiRequest
    {
        private readonly long _accountId;
        private readonly Guid _applicationId;

        public GetApplicationRequest(long accountId, Guid applicationId)
        {
            _accountId = accountId;
            _applicationId = applicationId;
        }

        public string GetUrl => $"accounts/{_accountId}/applications/{_applicationId}";
    }
}