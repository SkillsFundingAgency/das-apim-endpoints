using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetApprenticeshipsRequest : IGetApiRequest
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;

        public GetApprenticeshipsRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string BaseUrl { get; set; }
        public string Version { get; }

        public string GetUrl =>
            $"{BaseUrl}api/apprenticeships?accountId={_accountId}&accountLegalEntityId={_accountLegalEntityId}";
    }
}