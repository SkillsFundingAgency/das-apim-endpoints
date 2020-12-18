using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class UpdateCollectionCalendarPeriodCommand : IRequest
    {
        public byte PeriodNumber { get; }
        public string AcademicYear { get; }
        public bool Active { get; }

        public UpdateCollectionCalendarPeriodCommand(byte periodNumber, string academicYear, bool active)
        {
            PeriodNumber = periodNumber;
            AcademicYear = academicYear;
            Active = active;
        }
    }
}
