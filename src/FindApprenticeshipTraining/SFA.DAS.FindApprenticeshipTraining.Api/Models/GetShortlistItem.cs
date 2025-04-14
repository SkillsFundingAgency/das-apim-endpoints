using System;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    [Obsolete("FAT25 should be deleted eventually")]
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public GetTrainingCourseListItem Course { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreatedDate { get; set; }

        public static implicit operator GetShortlistItem(InnerApi.Responses.GetShortlistItem source)
        {
            return new GetShortlistItem
            {
                Id = source.Id,
                ShortlistUserId = source.ShortlistUserId,
                Course = source.Course,
                LocationDescription = source.LocationDescription,
                CreatedDate = source.CreatedDate
            };
        }
    }
}
