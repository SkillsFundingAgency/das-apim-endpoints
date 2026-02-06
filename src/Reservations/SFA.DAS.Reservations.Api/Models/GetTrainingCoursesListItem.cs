using System;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetTrainingCoursesListItem
    {
        public DateTime EffectiveTo { get; set; }

        public string Title { get; set; }

        public string Level { get; set; }

        public string Id { get; set; }

        public string ApprenticeshipType { get; set; }

        public string LearningType { get; set; }

        public static implicit operator GetTrainingCoursesListItem(TrainingCourseListItem course)
        {
            return new GetTrainingCoursesListItem
            {
                Id = course.LarsCode,
                Level = course.Level,
                Title = course.Title ?? string.Empty,
                EffectiveTo = course.EffectiveTo,
                ApprenticeshipType = course.ApprenticeshipType,
                LearningType = course.LearningType
            };
        }
    }
}