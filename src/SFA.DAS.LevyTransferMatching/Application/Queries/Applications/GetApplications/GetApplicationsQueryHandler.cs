using SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQueryHandler : GetApplicationsQueryHandlerBase<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : 
            base(levyTransferMatchingService, coursesApiClient)
        {
        }
    }
}