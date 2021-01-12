
namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class UpdateCollectionCalendarPeriodRequest
    {
        public byte PeriodNumber { get; set; }
        public short AcademicYear { get; set; }
        public bool Active { get; set; }
    }
}
