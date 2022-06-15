using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetTrainingProviderAdditionalCourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        
        public static implicit operator GetTrainingProviderAdditionalCourseListItem(GetAdditionalCourseListItem source)
        {
            return new GetTrainingProviderAdditionalCourseListItem
            {
                Id = source.Id,
                Level = source.Level,
                Title = source.Title
            };
        }
    }
}