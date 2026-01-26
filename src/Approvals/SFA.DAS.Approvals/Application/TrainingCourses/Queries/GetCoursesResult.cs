using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesResult
{
    public IEnumerable<TrainingProgramme> TrainingProgrammes { get; set; }
}