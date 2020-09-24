using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingCourseProvidersResponse
    {
        public GetTrainingCourseListItem TrainingCourse { get ; set ; }
        public int Total { get; set; }
        public IEnumerable<GetTrainingCourseProviderListItem> TrainingCourseProviders { get; set; }
        public GetLocationSearchResponseItem Location { get; set; }
    }
}
