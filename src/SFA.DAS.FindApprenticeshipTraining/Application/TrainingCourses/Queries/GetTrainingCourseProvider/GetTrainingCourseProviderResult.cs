using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderResult
    {
        public GetStandardsListItem Course { get ; set ; }
        public GetProviderStandardItem ProviderStandard { get; set; }
        public IEnumerable<GetAdditionalCourseListItem> AdditionalCourses { get ; set ; }
        public IEnumerable<GetAchievementRateItem> OverallAchievementRates { get; set; }
        public int TotalProviders { get ; set ; }
        public int TotalProvidersAtLocation { get ; set ; }
    }
}