using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;

public class GetCourseCodesResponse
{
    public IEnumerable<TrainingProgramme> TrainingProgrammes { get; set; }
}
