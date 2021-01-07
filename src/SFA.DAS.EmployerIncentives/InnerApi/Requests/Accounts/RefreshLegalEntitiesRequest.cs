using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts
{
    public class RefreshLegalEntitiesRequest : IPatchApiRequest<RefreshLegalEntitiesRequestData>
    {
        public string PatchUrl => "/legalentities/refresh";

        public RefreshLegalEntitiesRequestData Data { get; set; }
    }
}
