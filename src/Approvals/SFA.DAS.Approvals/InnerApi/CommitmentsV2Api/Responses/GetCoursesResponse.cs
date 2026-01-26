using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

public class GetCoursesResponse
{
    public IEnumerable<TrainingProgramme> TrainingProgrammes { get; set; }
}
