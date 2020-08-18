using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingAdditionalCourseItem
    {
        public int Total { get; set; }
        public List<GetTrainingProviderAdditionalCourseListItem> Courses { get; set; }
    }
}