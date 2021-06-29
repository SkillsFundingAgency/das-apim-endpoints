using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Tags
{
    public class GetJobRolesRequest : IGetAllApiRequest
    {
        public string GetAllUrl => "tags/jobRoles";
        public string Version { get; }
    }
}
