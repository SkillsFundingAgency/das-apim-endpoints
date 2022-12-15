using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{ 
    public class DeliveryModel
    {
        public LocationType LocationType { get; set; }
        public bool? DayRelease { get; set; }
        public bool? BlockRelease { get; set; }
        public decimal? DistanceInMiles { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
    }
}
