
namespace SFA.DAS.EmployerIncentives.Api.Models
{
    public class UpdateCollectionCalendarPeriodRequest
    {
        public byte PeriodNumber { get; set; }
        public string AcademicYear { get; set; }
        public bool Active { get; set; }
    }
}
