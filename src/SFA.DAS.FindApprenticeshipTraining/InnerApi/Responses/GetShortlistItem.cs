using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public GetProviderStandardItem ProviderDetails { get; set; }
        public int CourseId { get; set; }
    }
}