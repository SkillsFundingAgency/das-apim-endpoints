using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class UpdateCollectionCalendarPeriodCommand : IRequest
    {
        public byte PeriodNumber { get; }
        public short AcademicYear { get; }
        public bool Active { get; }

        public UpdateCollectionCalendarPeriodCommand(byte periodNumber, short academicYear, bool active)
        {
            PeriodNumber = periodNumber;
            AcademicYear = academicYear;
            Active = active;
        }
    }
}
