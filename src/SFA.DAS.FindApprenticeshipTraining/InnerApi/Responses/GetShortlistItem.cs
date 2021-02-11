using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public GetProviderStandardItem Provider { get; set; }//GetProvidersListItem
        public int CourseId { get; set; }
        public string LocationDescription { get; set; }

        [NotMapped]
        public GetStandardsListItem Course { get; set; }
    }
}