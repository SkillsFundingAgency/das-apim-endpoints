using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference
{
    public class GetJobRolesRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "reference/jobRoles";
    }
}
