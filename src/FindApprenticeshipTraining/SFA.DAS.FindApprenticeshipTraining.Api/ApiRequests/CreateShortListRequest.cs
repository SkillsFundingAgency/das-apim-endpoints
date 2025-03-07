using System;

namespace SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests
{
    public class CreateShortListRequest
    {
        public Guid ShortlistUserId { get; set; }
        public float? Lat { get; set; }
        public float? Lon { get; set; }
        public int LarsCode { get; set; }
        public string LocationDescription { get; set; }
        public int Ukprn { get; set; }
    }
}