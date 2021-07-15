using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetApprenticeshipIncentivesRequest : IGetAllApiRequest
    {
        private readonly long _accountId;
        private readonly long _accountLegalEntityId;

        public GetApprenticeshipIncentivesRequest(long accountId, long accountLegalEntityId)
        {
            _accountId = accountId;
            _accountLegalEntityId = accountLegalEntityId;
        }

        public string GetAllUrl => $"/accounts/{_accountId}/legalEntities/{_accountLegalEntityId}/apprenticeshipIncentives";
    }
}