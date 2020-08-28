using System.ComponentModel;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetDeliveryType
    {
        public DeliveryModeType DeliveryModeType { get; set; }
        public decimal DistanceInMiles { get; set; }
    }
    public enum DeliveryModeType
    {
        [Description("100PercentEmployer")]
        Workplace = 0,
        [Description("DayRelease")]
        DayRelease = 1,
        [Description("BlockRelease")]
        BlockRelease = 2
    }
}