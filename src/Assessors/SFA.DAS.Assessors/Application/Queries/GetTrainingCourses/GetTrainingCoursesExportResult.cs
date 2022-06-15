using System.Collections.Generic;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetTrainingCoursesExportResult
    {
        public IEnumerable<StandardDetailResponse> TrainingCourses { get ; set ; }
    }
}