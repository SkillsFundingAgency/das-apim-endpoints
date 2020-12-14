
namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class UpdateCollectionCalendarPeriodRequest
    {
        public byte CollectionPeriodNumber { get; set; }
        public short CollectionPeriodYear { get; set; }
        public bool Active { get; set; }
    }
}
