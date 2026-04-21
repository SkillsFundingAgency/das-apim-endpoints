using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference
{
    public class GetJobRolesRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "reference/jobRoles";
    }
}
