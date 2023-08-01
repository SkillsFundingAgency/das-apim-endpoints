using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses
{
    public class GetAllStandardsResponse
    {
        public IEnumerable<TrainingProgramme> TrainingProgrammes { get; set; }

        public class TrainingProgramme
        {
            public string CourseCode { get; set; }
            public string Name { get; set; }
        }
    }
}
