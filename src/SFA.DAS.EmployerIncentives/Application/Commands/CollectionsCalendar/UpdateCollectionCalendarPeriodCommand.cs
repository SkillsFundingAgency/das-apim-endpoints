using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CollectionsCalendar
{
    public class UpdateCollectionCalendarPeriodCommand : IRequest
    {
        public byte CollectionPeriodNumber { get; }
        public short CollectionPeriodYear { get; }
        public bool Active { get; }

        public UpdateCollectionCalendarPeriodCommand(byte collectionPeriodNumber, short collectionPeriodYear, bool active)
        {
            CollectionPeriodNumber = collectionPeriodNumber;
            CollectionPeriodYear = collectionPeriodYear;
            Active = active;
        }
    }
}
