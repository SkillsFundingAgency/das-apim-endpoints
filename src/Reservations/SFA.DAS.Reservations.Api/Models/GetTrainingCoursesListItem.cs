using System;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.Models
{
    public class GetTrainingCoursesListItem
    {
        public DateTime EffectiveTo { get ; set ; }

        public string Title { get ; set ; }

        public int Level { get ; set ; }

        public int Id { get ; set ; }

        public static implicit operator GetTrainingCoursesListItem(GetStandardsListItem standard)
        {
            return new GetTrainingCoursesListItem
            {
                Id = standard.LarsCode,
                Level = standard.Level,
                Title = standard.Title,
                EffectiveTo = standard.EffectiveTo
            };
        }
    }
}