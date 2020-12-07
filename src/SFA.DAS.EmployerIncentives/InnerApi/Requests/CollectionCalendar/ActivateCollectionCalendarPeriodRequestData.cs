
namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar
{
    public class ActivateCollectionCalendarPeriodRequestData
    {
        public byte CollectionPeriodNumber { get; set; }
        public short CollectionPeriodYear { get; set; }
        public bool Active { get; set; }
    }
}
