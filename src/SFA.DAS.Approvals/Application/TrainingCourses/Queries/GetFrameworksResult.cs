using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetFrameworksResult
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
        
    }
}