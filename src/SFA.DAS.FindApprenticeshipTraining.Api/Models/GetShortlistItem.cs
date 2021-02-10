using System;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public  GetTrainingCourseProviderListItem Provider { get; set; }
        public GetTrainingCourseListItem Course { get; set; }
        public string LocationDescription { get; set; }

        public static implicit operator GetShortlistItem(InnerApi.Responses.GetShortlistItem source)
        {
            var provider = new GetTrainingCourseProviderListItem().Map(
                source.Provider,
                source.Course.Route,
                source.Course.Level,
                null,
                null,
                !string.IsNullOrEmpty(source.LocationDescription));

            return new GetShortlistItem
            {
                Id = source.Id,
                ShortlistUserId = source.ShortlistUserId,
                Provider = provider,
                Course = source.Course,
                LocationDescription = source.LocationDescription
            };
        }
    }
}
