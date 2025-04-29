using System;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public GetProviderCourseItem Provider { get; set; }
        public GetTrainingCourseListItem Course { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreatedDate { get; set; }

        public static implicit operator GetShortlistItem(InnerApi.Responses.GetShortlistItem source)
        {
            var provider = new GetProviderCourseItem().Map(source);

            return new GetShortlistItem
            {
                Id = source.Id,
                ShortlistUserId = source.ShortlistUserId,
                Provider = provider,
                Course = source.Course,
                LocationDescription = source.LocationDescription,
                CreatedDate = source.CreatedDate
            };
        }
    }
}
