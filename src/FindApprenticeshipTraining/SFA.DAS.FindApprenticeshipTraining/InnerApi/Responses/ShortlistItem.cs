using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class ShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public int Ukprn { get; set; }
        public int Larscode { get; set; }
        public string LocationName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
