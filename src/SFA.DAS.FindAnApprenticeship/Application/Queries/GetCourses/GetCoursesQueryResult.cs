using System.Collections.Generic;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses
{
    public class GetCoursesQueryResult
    {
        public IEnumerable<TrainingProgramme> TrainingProgrammes { get; set; }
    }
}