using System.Collections.Generic;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetTrainingCoursesResult
    {
        public IEnumerable<GetStandardsListItem> TrainingCourses { get ; set ; }
    }
}