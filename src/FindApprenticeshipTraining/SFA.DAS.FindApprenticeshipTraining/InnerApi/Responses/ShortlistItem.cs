using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class ShortlistItem
    {
        public Guid Id { get; set; }
        public Guid ShortlistUserId { get; set; }
        public int Ukprn { get; set; }
        public int Larscode { get; set; }
        public string LocationDescription { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
