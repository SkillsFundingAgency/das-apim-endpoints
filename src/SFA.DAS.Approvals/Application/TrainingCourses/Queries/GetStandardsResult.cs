using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetStandardsResult
    {
        public IEnumerable<GetStandardsListItem> Standards { get ; set ; }
    }
}