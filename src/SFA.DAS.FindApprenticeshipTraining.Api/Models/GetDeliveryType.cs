using System.ComponentModel;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetDeliveryType
    {
        public DeliveryModeType DeliveryModeType { get; set; }
        public decimal DistanceInMiles { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public bool National { get ; set ; }
    }
    public enum DeliveryModeType
    {
        [Description("100PercentEmployer")]
        Workplace = 0,
        [Description("DayRelease")]
        DayRelease = 1,
        [Description("BlockRelease")]
        BlockRelease = 2,
        [Description("NotFound")]
        NotFound = 3,
        [Description("National")]
        National = 4
    }
}