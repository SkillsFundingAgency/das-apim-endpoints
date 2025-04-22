using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    [Obsolete("FAT25 should be deleted eventually")]
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public GetProviderStandardItem ProviderDetails { get; set; }
        public int CourseId { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public GetStandardsListItem Course { get; set; }
    }
}